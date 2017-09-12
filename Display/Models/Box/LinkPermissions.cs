using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Display.Models.Box
{
    public class LinkPermissions
    {
        [JsonProperty("can_download")]
        public bool CanDownload { get; set; }

        [JsonProperty("can_preview")]
        public bool CanPreview { get; set; }
    }
}