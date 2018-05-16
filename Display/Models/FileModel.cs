using MongoDB.Bson;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Display.Models
{
    public class FileModel
    {
        public ObjectId ContentId { get; set; }
        public string FileName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Active { get; set; }

        public static async Task<FileModel> Create(string fileName, Stream stream)
        {
            var result = new FileModel();

            result.ContentId = await FileRepository.Current.SaveFile(fileName, stream);

            result.FileName = fileName;

            using (var img = new Bitmap(stream))
            {
                result.Width = img.Width;
                result.Height = img.Height;
            }

            result.Active = true;

            return result;
        }
    }
}