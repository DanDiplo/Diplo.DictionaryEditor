using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Diplo.Dictionary.Models
{
    /// <summary>
    /// Represents individual cmsLanguage table entries
    /// </summary>
    /// <remarks>
    /// This is basically a copy of the internal Umbraco  class
    /// https://github.com/umbraco/Umbraco-CMS/blob/7ee510ed386495120666a78c61497f58ff05de8f/src/Umbraco.Core/Models/Rdbms/LanguageTextDto.cs
    /// </remarks>
    [TableName("cmsLanguageText")]
    [PrimaryKey("pk")]
    [ExplicitColumns]
    public class LanguageTextDto
    {
        [Column("pk")]
        [PrimaryKeyColumn]
        public int PrimaryKey { get; set; }

        [Column("languageId")]
        public int LanguageId { get; set; }

        [Column("UniqueId")]
        public Guid UniqueId { get; set; }

        [Column("value")]
        [Length(1000)]
        public string Value { get; set; }
    }
}