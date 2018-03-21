using System;
using System.Collections.Generic;
using System.Linq;

namespace Diplo.Dictionary.Models
{
    /// <summary>
    /// Response from validating CSV
    /// </summary>
    public class ValidationResponse
    {
        public ValidationResponse()
        {
            this.Errors = new List<string>();
        }

        public bool IsValid { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public bool HasErrors => this.Errors != null && this.Errors.Any();

        public int RowsFound { get; set; }

        internal List<CsvRow> CsvRows { get; set; }

        internal bool HasCsvRows => this.CsvRows != null && this.CsvRows.Any();
    }
}