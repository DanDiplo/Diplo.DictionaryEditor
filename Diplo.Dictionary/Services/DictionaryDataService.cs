using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplo.Dictionary.Extensions;
using Diplo.Dictionary.Models;
using Diplo.Dictionary.Models.Json;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Diplo.Dictionary.Services
{
    public class DictionaryDataService
    {
        private readonly ApplicationContext appContext;
        private readonly ILocalizationService localisationService;
        private readonly IsolatedRuntimeCache isolatedRuntimeCache;
        private readonly Database db;

        public DictionaryDataService(ApplicationContext applicationContext)
        {
            this.appContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            this.localisationService = applicationContext.Services.LocalizationService;
            this.db = applicationContext.DatabaseContext.Database;
            this.isolatedRuntimeCache = applicationContext.ApplicationCache.IsolatedRuntimeCache;
        }

        /// <summary>
        /// Gets a collection of dictionary items, sorted by parent
        /// </summary>
        /// <returns>An collection of dictionary items</returns>
        public IEnumerable<DictItem> GetAllDictionaryItemsSorted()
        {
            List<IDictionaryItem> sortedList = new List<IDictionaryItem>();

            var rootItems = this.localisationService.GetRootDictionaryItems().OrderBy(x => x.ItemKey);

            foreach (var root in rootItems)
            {
                sortedList.Add(root);

                var children = this.localisationService.GetDictionaryItemChildren(root.Key).OrderBy(x => x.ItemKey);

                foreach (var child in children)
                {
                    sortedList.Add(child);
                    sortedList.AddRange(this.localisationService.GetDictionaryItemDescendants(child.Key).OrderBy(x => x.ItemKey));
                }
            }

            return sortedList.ToDictionaryItems();
        }

        /// <summary>
        /// Updates a collection of dictionary items (in the database) where a translation is flagged as IsUpdated
        /// </summary>
        /// <param name="dictionary">The dictionary to update</param>
        /// <returns>A count of how many items were updated</returns>
        public UpdateResponse UpdateChangedDictionaryItems(IEnumerable<DictItem> dictionary)
        {
            var dtos = dictionary.SelectMany(d => d.Translations.Where(t => t.IsUpdated).Select(t => t.ToLanguageTextDto()));

            if (dtos == null || !dtos.Any())
            {
                return new UpdateResponse()
                {
                    Message = "No dictionary items have been modified - nothing to update!"
                };
            }

            return UpdateDtoItems(dtos);
        }

        public UpdateResponse UpdateDtoItems(IEnumerable<LanguageTextDto> dtos)
        {
            UpdateResponse response = new UpdateResponse();

            if (dtos == null || !dtos.Any())
            {
                response.Message = "No dictionary items were found to update.";
                return response;
            }

            int itemsCount = dtos.Count();

            LogHelper.Info<DictionaryDataService>($"About to update {itemsCount} dictionary items...");

            foreach (var dto in dtos)
            {
                try
                {
                    db.Update(dto);
                    response.UpdateCount++;
                }
                catch (Exception ex)
                {
                    response.Errors.Add($"Error updating database for row Id '{dto.PrimaryKey}': {ex.Message}");
                    LogHelper.Error<DictionaryDataService>($"Error updating database for row Id {dto.PrimaryKey}, Language Id {dto.LanguageId}, with value '{dto.Value}'", ex);
                }
            }

            int errorCount = response.Errors.Count;

            if (errorCount == 0)
            {
                response.Message = $"Succesfully updated {response.UpdateCount} dictionary items.";
                LogHelper.Info<DictionaryDataService>(response.Message);
                response.IsSuccess = true;
            }
            else
            {
                response.Message = $"Updated {response.UpdateCount} dictionary items (out of {itemsCount}) but there were also {errorCount} items that could not be imported.";
                LogHelper.Warn<DictionaryDataService>(response.Message);
            }

            this.isolatedRuntimeCache.ClearCache<ILanguage>();

            return response;
        }
    }
}
