using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.Drawing;
using System.Threading.Tasks;

namespace Display.Models
{
    public class FileModel
    {
        private ObjectId _id;
        private string _fileName;
        private int _displayId;
        private int _width;
        private int _height;
        private byte[] _data;

        private FileModel() { }

        public static async Task<FileModel> Create(GridFSFileInfo fileInfo)
        {
            using (var stream = await FileRepository.Current.DownloadFile(fileInfo.Id))
            {
                var result = new FileModel();
                result._id = fileInfo.Id;
                result._fileName = fileInfo.Filename;
                result._displayId = fileInfo.Metadata["displayId"].ToInt32();

                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    result._data = new byte[stream.Length];
                    stream.Read(result._data, 0, (int)stream.Length);

                    using (var img = new Bitmap(stream))
                    {
                        result._width = img.Width;
                        result._height = img.Height;
                    }
                }
                else
                {
                    result._data = new byte[0];
                    result._width = 0;
                    result._height = 0;
                }

                return result;
            }
        }

        public string ID { get { return _id.ToString(); } }

        public int DisplayID { get { return _displayId; } }

        public string FileName { get { return _fileName; } }

        public byte[] Data { get { return _data; } }

        public int Width { get { return _width; } }

        public int Height { get { return _height; } }

        public async Task<bool> Delete()
        {
            return await FileRepository.Current.DeleteFile(_id);
        }
    }
}