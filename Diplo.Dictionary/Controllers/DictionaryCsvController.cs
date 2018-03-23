using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplo.Dictionary.Models;
using Diplo.Dictionary.Services;
using Diplo.Dictionary.Extensions;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi.Filters;
using Diplo.Dictionary.Sections;
using Umbraco.Web.WebApi;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Diplo.Dictionary.Controllers
{
    /// <summary>
    /// Controller for importing and exporting dictionary items as CSV
    /// </summary>
    [UmbracoApplicationAuthorize(DiploConstants.SectionAlias)]
    [PluginController(DiploConstants.PluginName)]
    public class DictionaryCsvController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// Exports dictionary items as CSV
        /// </summary>
        /// <returns>A file result</returns>
        [System.Web.Http.HttpGet]
        public IHttpActionResult ExportAsCsv(int? lang)
        {
            var dictionaryService = new DictionaryDataService(ApplicationContext);

            var csv = dictionaryService.GetAllDictionaryItemsSorted().ToCsv(lang);

            string fileName = "dictionary-" + DateTime.Today.ToString("yyyyMMdd") + ".csv";

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(Encoding.UTF8.GetBytes(csv))
            };

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName,
            };

            result.Content.Headers.ContentLength = Encoding.UTF8.GetByteCount(csv);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

            var response = ResponseMessage(result);

            return response;
        }

        /// <summary>
        /// Import Dictionary as CSV
        /// </summary>
        /// <returns>A response</returns>
        [System.Web.Http.HttpPost]
        public async Task<ImportResponse> ImportCsv()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            ImportResponse response = new ImportResponse();

            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.FirstOrDefault();

            if (file != null)
            {
                response.Filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                
                string csv = await file.ReadAsStringAsync();

                var csvService = new CsvService(ApplicationContext);

                response = csvService.ImportCsv(csv);
            }

            return response;
        }
    }
}
