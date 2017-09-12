using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Display.Models.Box
{
    public class PathCollection
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("entries")]
        public IEnumerable<PathEntry> Entries { get; set; }
    }
}