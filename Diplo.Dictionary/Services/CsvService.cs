using CsvHelper;
using Diplo.Dictionary.Extensions;
using Diplo.Dictionary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Core;

namespace Diplo.Dictionary.Services
{
	public class CsvService
	{
		private readonly ApplicationContext appContext;
		private static readonly string[] NewLineChars = new[] { "\r\n", "\r", "\n" };
		private static readonly Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.Compiled); // naive CSV parser

		/// <summary>
		/// Instantiate the CSV Service from the Application Context
		/// </summary>
		/// <param name="applicationContext">The Umbraco Application Context</param>
		public CsvService(ApplicationContext applicationContext)
		{
			this.appContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
		}

		/// <summary>
		/// Empty constructor for tests
		/// </summary>
		public CsvService() { }

		/// <summary>
		/// Exports entire dictionary to CSV
		/// </summary>
		/// <returns>A string containg the CSV items</returns>
		public string ExportDictionaryToCsv()
		{
			var dictionaryService = new DictionaryDataService(appContext);

			var csv = dictionaryService.GetAllDictionaryItemsSorted().ToCsv();

			return csv;
		}

		/// <summary>
		/// Validates the passed CSV to ensure it is the correct format
		/// </summary>
		/// <remarks>
		/// This validates the format and not whether the items exist
		/// </remarks>
		/// <param name="csv">The CSV to validate</param>
		/// <returns>A response</returns>
		public ValidationResponse ValidateCsv(string csv)
		{
			if (String.IsNullOrEmpty(csv))
			{
				return new ValidationResponse()
				{
					Message = "The CSV file didn't contain any data so cannot be parsed"
				};
			}

			ValidationResponse response = new ValidationResponse();
			response.CsvRows = new List<CsvRow>(response.RowsFound);

			int i = 0;

			using (var reader = new StringReader(csv))
			using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csvReader.Read();
				csvReader.ReadHeader();
				while (csvReader.Read())
				{
					CsvRow row = new CsvRow();

					if (int.TryParse(csvReader.GetField<string>(0), out int id))
					{
						row.Id = id;
					}
					else
					{
						response.Errors.Add($"The CSV line at row {i} for column 'Id' didn't contain a number as expected - it contained '{csvReader.GetField<string>(0)}'.");
					}

					if (Guid.TryParse(csvReader.GetField<string>(1), out Guid guid))
					{
						row.Key = guid;
					}
					else
					{
						response.Errors.Add($"The CSV line at row {i} for column 'Key' didn't contain a GUID as expected - it contained '{csvReader.GetField<string>(1)}'.");
					}

					if (int.TryParse(csvReader.GetField<string>(2), out id))
					{
						row.LangId = id;
					}
					else
					{
						response.Errors.Add($"The CSV line at row {i} for column 'LangId' didn't contain a number as expected - it contained '{csvReader.GetField<string>(2)}'.");
					}

					row.Language = csvReader.GetField<string>(3).Trim("\"");
					row.Name = csvReader.GetField<string>(4).Trim("\"");
					row.Translation = csvReader.GetField<string>(5).Trim("\"");

					response.CsvRows.Add(row);

					i++;
				}
			}

			response.RowsFound = i;

			if (response.Errors.Any())
			{
				response.IsValid = false;
				response.Message = "Your CSV file had a number of errors trying to read the file.";
			}
			else
			{
				response.IsValid = true;
			}

			return response;
		}

		/// <summary>
		/// Imports the CSV dictionary back into Umbraco
		/// </summary>
		/// <param name="csv">The CSV entries</param>
		/// <returns></returns>
		public ImportResponse ImportCsv(string csv)
		{
			ImportResponse response = new ImportResponse
			{
				Validation = this.ValidateCsv(csv)
			};

			if (!response.Validation.IsValid)
			{
				return response;
			}

			var dtos = response.Validation.CsvRows.ToLanguageTextDtos();

			var dictionaryService = new DictionaryDataService(appContext);

			response.Update = dictionaryService.UpdateDtoItems(dtos);

			return response;
		}
	}
}
