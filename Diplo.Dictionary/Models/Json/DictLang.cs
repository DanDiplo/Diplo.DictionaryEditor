using System;
using Newtonsoft.Json;

namespace Diplo.Dictionary.Models.Json
{
    [JsonObject(Title = "Language")]
    public class DictLang
    {
        [JsonProperty("IsoCode")]
        public string IsoCode { get; set; }

        [JsonProperty("CultureName")]
        public string CultureName { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Key")]
        public Guid Key { get; set; }
    }
}
