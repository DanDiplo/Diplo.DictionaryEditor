using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Diplo.Dictionary.Extensions;
using Diplo.Dictionary.Models;
using Diplo.Dictionary.Models.Json;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;

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

            var rows = csv.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
            response.RowsFound = rows.Count();
            response.CsvRows = new List<CsvRow>(response.RowsFound);

            int i = 1;

            foreach (var line in rows)
            {
                var columns = CSVParser.Split(line);
                int columnCount = columns.Count();

                if (columns.Count() != 6)
                {
                    response.Errors.Add($"The CSV line at row {i} contained {columnCount} columns where it should have contained 6.");
                }
                else
                {
                    CsvRow row = new CsvRow();

                    if (int.TryParse(columns[0].Trim("\""), out int id))
                    {
                        row.Id = id;
                    }
                    else
                    {
                        response.Errors.Add($"The CSV line at row {i} for column 'Id' didn't contain a number as expected - it contained '{columns[0]}'.");
                    }

                    if (Guid.TryParse(columns[1].Trim("\""), out Guid guid))
                    {
                        row.Key = guid;
                    }
                    else
                    {
                        response.Errors.Add($"The CSV line at row {i} for column 'Key' didn't contain a GUID as expected - it contained '{columns[1]}'.");
                    }

                    if (int.TryParse(columns[2].Trim("\""), out id))
                    {
                        row.LangId = id;
                    }
                    else
                    {
                        response.Errors.Add($"The CSV line at row {i} for column 'LangId' didn't contain a number as expected - it contained '{columns[2]}'.");
                    }

                    row.Language = columns[3].Trim("\"");
                    row.Name = columns[4].Trim("\"");
                    row.Translation = columns[5].Trim("\"");

                    response.CsvRows.Add(row);
                }

                i++;
            }

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
