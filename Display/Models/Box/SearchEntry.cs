using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Display.Models.Box
{
    public class SearchEntry : PathEntry
    {
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("size")]
        public long size { get; set; }

        [JsonProperty("path_collection")]
        public PathCollection PathCollection { get; set; }

        [JsonProperty("created_by")]
        public User CreatedBy { get; set; }

        [JsonProperty("trashed_at")]
        public DateTime? TrashedAt { get; set; }

        [JsonProperty("purged_at")]
        public DateTime? PurgedAt { get; set; }

        [JsonProperty("content_created_at")]
        public DateTime? ContentCreatedAt { get; set; }

        [JsonProperty("content_modified_at")]
        public DateTime? ContentModifiedAt { get; set; }

        [JsonProperty("owned_by")]
        public User OwnedBy { get; set; }

        [JsonProperty("shared_link")]
        public SharedLink SharedLink { get; set; }

        [JsonProperty("folder_upload_email")]
        public string FolderUploadEmail { get; set; }

        [JsonProperty("parent")]
        public PathEntry Parent { get; set; }

        [JsonProperty("item_status")]
        public string ItemStatus { get; set; }
    }
}