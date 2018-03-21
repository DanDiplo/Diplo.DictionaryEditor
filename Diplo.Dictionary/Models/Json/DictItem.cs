using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Diplo.Dictionary.Models.Json
{
    public class DictItem
    {
        public int Depth { get; set; }

        [JsonProperty("ParentId")]
        public Guid? ParentId { get; set; }

        [JsonProperty("ItemKey")]
        public string ItemKey { get; set; }

        [JsonProperty("Translations")]
        public IEnumerable<DictTrans> Translations { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Key")]
        public Guid Key { get; set; }
    }
}
