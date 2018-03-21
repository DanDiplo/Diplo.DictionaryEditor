using System;
using System.Collections.Generic;
using System.Web.Http;
using Diplo.Dictionary.Models;
using Diplo.Dictionary.Models.Json;
using Diplo.Dictionary.Sections;
using Diplo.Dictionary.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi.Filters;

namespace Diplo.Dictionary.Controllers
{
    /// <summary>
    /// Controller for getting and updating dictionary items
    /// </summary>
    [UmbracoApplicationAuthorize(DiploConstants.SectionAlias)]
    [PluginController(DiploConstants.PluginName)]
    public class DictionaryEditorController : UmbracoAuthorizedJsonController
    {
        private readonly DictionaryDataService dictionaryService;

        /// <summary>
        /// Instantiates the controller and configures the dictionary data service
        /// </summary>
        public DictionaryEditorController()
        {
            this.dictionaryService = new DictionaryDataService(ApplicationContext);
        }

        /// <summary>
        /// Endpoint to gets all dictionary and translation values (sorted correctly!)
        /// </summary>
        /// <returns>An array of dictionary items as JSON</returns>
        public IEnumerable<DictItem> GetEntireDictionary()
        {
            return this.dictionaryService.GetAllDictionaryItemsSorted();
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

            return this.dictionaryService.UpdateChangedDictionaryItems(dictionary);
        }

        /// <summary>
        /// Endpoint to get all configured lanaguages
        /// </summary>
        /// <returns>A list of languages as JSON</returns>
        public IEnumerable<DictLang> GetLanguages()
        {
            return this.dictionaryService.GetAllLanguages();
        }
    }
}