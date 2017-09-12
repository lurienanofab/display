using System.Collections.Generic;

namespace Display.Models
{
    public class AdminModel
    {
        public int ID { get; set; }
        public IEnumerable<FileModel> Files { get; set; }
    }
}