using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Display.Models.Box
{
    public class SharedLink
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("vanity_url")]
        public string VanityUrl { get; set; }

        [JsonProperty("effective_access")]
        public string EffectiveAccess { get; set; }

        [JsonProperty("is_password_enabled")]
        public bool IsPasswordEnabled { get; set; }

        [JsonProperty("unshared_at")]
        public DateTime? UnsharedAt { get; set; }

        [JsonProperty("download_count")]
        public int DownloadCount { get; set; }

        [JsonProperty("preview_count")]
        public int PreviewCount { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }

        [JsonProperty("permissions")]
        public LinkPermissions Permissions { get; set; }
    }
}