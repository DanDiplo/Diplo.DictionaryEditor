using System;
using Newtonsoft.Json;

namespace Diplo.Dictionary.Models.Json
{
    [JsonObject(Title = "Translation")]
    public class DictTrans
    {
        [JsonProperty("Language")]
        public DictLang Language { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Key")]
        public Guid Key { get; set; }

        [JsonProperty("IsUpdated")]
        public bool IsUpdated { get; set; }
    }
}
