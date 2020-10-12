using Diplo.Dictionary.Models;
using Diplo.Dictionary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Diplo.Dictionary.Tests
{
	[TestClass]
	public class CsvServiceTests
	{
		private readonly CsvService _service;

		public CsvServiceTests()
		{
			_service = new CsvService();
		}

		[TestMethod]
		public void Should_Parse_With_Multiline_Cells()
		{

			string source = $"Id,Key,LangId,Language,Name,Translation{Environment.NewLine}" +
				$"1991,f1900d0e-ba19-45c7-87ca-bec18c03dc80,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet. {Environment.NewLine}" +
				"consectetur adipiscing elit\"";

			ValidationResponse validationResponse = _service.ValidateCsv(source);

			Assert.IsTrue(validationResponse.IsValid);
			Assert.AreEqual(1, validationResponse.RowsFound);
		}

		[TestMethod]
		public void Fails_Cause_Id_Invalid()
		{
			string source = $"Id,Key,LangId,Language,Name,Translation{Environment.NewLine}" +
				$"asdf,f1900d0e-ba19-45c7-87ca-bec18c03dc80,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet. {Environment.NewLine}" +
				"consectetur adipiscing elit\"";

			ValidationResponse validationResponse = _service.ValidateCsv(source);

			Assert.IsFalse(validationResponse.IsValid);
		}

		[TestMethod]
		public void Fails_Cause_Guid_Invalid()
		{
			string source = $"Id,Key,LangId,Language,Name,Translation{Environment.NewLine}" +
				$"1991,123,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet. {Environment.NewLine}" +
				"consectetur adipiscing elit\"";

			ValidationResponse validationResponse = _service.ValidateCsv(source);

			Assert.IsFalse(validationResponse.IsValid);
		}

		[TestMethod]
		public void Fails_Cause_LangId_Invalid()
		{
			string source = $"Id,Key,LangId,Language,Name,Translation{Environment.NewLine}" +
				$"1991,f1900d0e-ba19-45c7-87ca-bec18c03dc80,asdf,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet. {Environment.NewLine}" +
				"consectetur adipiscing elit\"";

			ValidationResponse validationResponse = _service.ValidateCsv(source);

			Assert.IsFalse(validationResponse.IsValid);
		}

		[TestMethod]
		public void Simple_Parsing_Test()
		{
			string source = $"Id,Key,LangId,Language,Name,Translation{Environment.NewLine}" +
				$"1991,f1900d0e-ba19-45c7-87ca-bec18c03dc80,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet.\"{Environment.NewLine}" +
				$"1991,f1900d0e-ba19-45c7-87ca-bec18c03dc80,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet.\"{Environment.NewLine}" +
				$"1991,f1900d0e-ba19-45c7-87ca-bec18c03dc80,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet.\"{Environment.NewLine}" +
				$"1991,f1900d0e-ba19-45c7-87ca-bec18c03dc80,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet.\"{Environment.NewLine}" +
				$"1991,f1900d0e-ba19-45c7-87ca-bec18c03dc80,2,French (Switzerland),Dictionary.Key,\"Lorem ipsum dolor sit amet.\"";

			ValidationResponse validationResponse = _service.ValidateCsv(source);

			Assert.IsTrue(validationResponse.IsValid);
			Assert.AreEqual(5, validationResponse.RowsFound);
		}
	}
}
