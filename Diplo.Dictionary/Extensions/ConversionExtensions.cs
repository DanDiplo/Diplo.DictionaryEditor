using System;
using System.Collections.Generic;
using System.Linq;
using Diplo.Dictionary.Models;
using Diplo.Dictionary.Models.Json;
using Umbraco.Core.Models;

namespace Diplo.Dictionary.Extensions
{
    /// <summary>
    /// Extension methods for converting between types
    /// </summary>
    public static class ConversionExtensions
    {
        public static DictItem ToDictItem(this IDictionaryItem dict)
        {
            return new DictItem()
            {
                Id = dict.Id,
                ItemKey = dict.ItemKey,
                Key = dict.Key,
                ParentId = dict.ParentId,
                Translations = dict.Translations.ToDictTranslations()
            };
        }

        public static DictLang ToDictLang(this ILanguage lang)
        {
            return new DictLang()
            {
                CultureName = !String.IsNullOrEmpty(lang.CultureName) ? lang.CultureName : lang.IsoCode,
                Id = lang.Id,
                IsoCode = lang.IsoCode,
                Key = lang.Key
            };
        }

        public static DictTrans ToDictTrans(this IDictionaryTranslation trans)
        {
            return new DictTrans()
            {
                Id = trans.Id,
                Key = trans.Key,
                Language = trans.Language.ToDictLang(),
                Value = trans.Value
            };
        }

        public static IEnumerable<DictTrans> ToDictTranslations(this IEnumerable<IDictionaryTranslation> translations)
        {
            if (translations == null)
                return Enumerable.Empty<DictTrans>();

            return translations.Select(t => t.ToDictTrans());
        }

        public static IEnumerable<DictItem> ToDictionaryItems(this IEnumerable<IDictionaryItem> dictionaryItems)
        {
            if (dictionaryItems == null)
                return Enumerable.Empty<DictItem>();

            return dictionaryItems.Select(di => di.ToDictItem());
        }

        public static LanguageTextDto ToLanguageTextDto(this DictTrans translation)
        {
            LanguageTextDto dto = new LanguageTextDto()
            {
                LanguageId = translation.Language.Id,
                PrimaryKey = translation.Id,
                UniqueId = translation.Key,
                Value = translation.Value
            };

            return dto;
        }

        public static LanguageTextDto ToLanguageTextDto(this CsvRow row)
        {
            LanguageTextDto dto = new LanguageTextDto()
            {
                LanguageId = row.LangId,
                PrimaryKey = row.Id,
                UniqueId = row.Key,
                Value = row.Translation
            };

            return dto;
        }

        public static IEnumerable<LanguageTextDto> ToLanguageTextDtos(this IEnumerable<CsvRow> csvRows)
        {
            if (csvRows == null)
                return Enumerable.Empty<LanguageTextDto>();

            return csvRows.Select(r => r.ToLanguageTextDto());
        }
    }
}