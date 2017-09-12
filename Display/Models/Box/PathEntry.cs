using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Display.Models.Box
{
    public class PathEntry : BoxObject
    {
        [JsonProperty("sequence_id")]
        public int? SequenceId { get; set; }

        [JsonProperty("etag")]
        public int? Etag { get; set; }
    }
}