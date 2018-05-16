using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Display.Models
{
    public class DisplayModel
    {
        public ObjectId Id { get; set; }
        public int DisplayID { get; set; }
        public DateTime LastUpdateUTC { get; set; }
        public IEnumerable<FileModel> Files { get; set; }
    }
}