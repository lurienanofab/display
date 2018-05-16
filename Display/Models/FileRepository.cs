using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Display.Models
{
    public class FileRepository : RepositoryBase
    {
        private static readonly FileRepository _Current;

        static FileRepository()
        {
            _Current = new FileRepository();
        }

        public static FileRepository Current
        {
            get { return _Current; }
        }

        public override string DatabaseName
        {
            get { return "display"; }
        }

        public async Task<byte[]> DownloadFile(ObjectId id)
        {
            var result = await GetBucket().DownloadAsBytesAsync(id);
            return result;
        }

        public async Task<IEnumerable<GridFSFileInfo>> GetFiles()
        {
            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Empty;
            var files = await GetBucket().Find(filter).ToListAsync();
            var result = new List<string>();
            return files;
        }

        public async Task<GridFSFileInfo> GetFileInfo(ObjectId id)
        {
            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Eq("_id", id);
            var file = await GetBucket().Find(filter).FirstOrDefaultAsync();
            return file;
        }

        public async Task<ObjectId> SaveFile(string fileName, Stream source)
        {
            var result = await GetBucket().UploadFromStreamAsync(fileName, source);
            return result;
        }

        public async Task<bool> DeleteFile(ObjectId id)
        {
            try
            {
                await GetBucket().DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAllFiles()
        {
            try
            {
                await GetBucket().DropAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}