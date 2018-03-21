using System;

namespace Diplo.Dictionary.Models
{
    /// <summary>
    /// Represents a single row in a CSV export
    /// </summary>
    public class CsvRow
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        public int LangId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public string Translation { get; set; }
    }
}