using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Display.Models.Box
{
    public class FolderEntry : SearchEntry
    {
        [JsonProperty("item_collection")]
        public PathCollection ItemCollection { get; set; }
    }
}