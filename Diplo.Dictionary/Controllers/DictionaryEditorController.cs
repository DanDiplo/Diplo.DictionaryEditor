using System;
using System.Collections.Generic;
using System.Linq;
using Diplo.Dictionary.Sections;
using Umbraco.Core;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi.Filters;
using Umbraco.Core.Models;
using System.Web.Http;
using Newtonsoft.Json;
using Diplo.Dictionary.Models.Json;
using Diplo.Dictionary.Extensions;
using Diplo.Dictionary.Models;
using Diplo.Dictionary.Services;

namespace Diplo.Dictionary.Controllers
{
    /// <summary>
    /// Controller for getting and updating dictionary items
    /// </summary>
    [UmbracoApplicationAuthorize(DiploConstants.SectionAlias)]
    [PluginController(DiploConstants.PluginName)]
    public class DictionaryEditorController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Endpoint to gets all dictionary and translation values (sorted correctly!)
        /// </summary>
        /// <returns>An array of dictionary items as JSON</returns>
        public IEnumerable<DictItem> GetEntireDictionary()
        {
            DictionaryDataService dictionaryService = new DictionaryDataService(ApplicationContext);

            return dictionaryService.GetAllDictionaryItemsSorted();
        }

        /// <summary>
        /// Endpoint to POST updated dictionary values.
        /// </summary>
        /// <remarks>
        /// Only those translations marked as updated are updated in the database
        /// </remarks>
        /// <param name="dictionary">The dictionary items to update</param>
        /// <returns>A response string</returns>
        [HttpPost]
        public UpdateResponse UpdateDictionary([FromBody] IEnumerable<DictItem> dictionary)
        {
            if (dictionary == null)
            {
                return new UpdateResponse()
                {
                    Message = "Cannot update - the dictionary was null!"
                };
            }

            DictionaryDataService dictionaryService = new DictionaryDataService(ApplicationContext);

            return dictionaryService.UpdateChangedDictionaryItems(dictionary);
        }

        /// <summary>
        /// Endpoint to get all configured lanaguages
        /// </summary>
        /// <returns>A list of languages as JSON</returns>
        public IEnumerable<ILanguage> GetLanguages()
        {
            return Services.LocalizationService.GetAllLanguages() ?? Enumerable.Empty<ILanguage>();
        }
    }
}
