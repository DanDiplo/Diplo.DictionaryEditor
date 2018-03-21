using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplo.Dictionary.Models;
using Diplo.Dictionary.Models.Json;

namespace Diplo.Dictionary.Extensions
{
    /// <summary>
    /// CSV Extensions
    /// </summary>
    /// <remarks>
    /// This is a naive implementation, but saves importing a proper CSV library
    /// </remarks>
    public static class CsvExtensions
    {
        /// <summary>
        /// Exports the dictionary items to a CSV file that can be parsed
        /// </summary>
        /// <param name="dictionaryItems">The dictionary to convert</param>
        /// <returns>The CSV file contents</returns>
        public static string ToCsv(this IEnumerable<DictItem> dictionaryItems, int? lang = null)
        {
            var dictionary = dictionaryItems.SelectMany(d => d.Translations);

            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Id,Key,LangId,Language,Name,Translation");

            foreach (var dict in dictionaryItems)
            {
                foreach (var item in dict.Translations.Where(l => lang.HasValue && lang.Value == l.Language.Id || lang == null))
                {
                    csv.AppendFormat("{0},{1},{2},{3},\"{4}\",\"{5}\"\n", 
                        item.Id, 
                        item.Key, 
                        item.Language.Id, 
                        item.Language.CultureName, 
                        dict.ItemKey, 
                        item.Value?.Replace("\"\"", "\"\"").Replace("\n", "").Trim());
                }
            }

            return csv.ToString();
        }
    }
}
