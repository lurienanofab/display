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

        public async Task<Stream> DownloadFile(ObjectId id)
        {
            MemoryStream ms = new MemoryStream();
            await GetBucket().DownloadToStreamAsync(id, ms);
            return ms;
        }

        public async Task<IEnumerable<FileModel>> GetFiles(int displayId)
        {
            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Eq(x => x.Metadata["displayId"], displayId);
            var files = await GetBucket().Find(filter).ToListAsync();
            var result = new List<FileModel>();
            foreach (var f in files)
                result.Add(await FileModel.Create(f));
            return result;
        }

        public async Task<FileModel> GetFile(ObjectId id)
        {
            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Eq("_id", id);
            var file = await GetBucket().Find(filter).FirstOrDefaultAsync();
            return await FileModel.Create(file);
        }

        public async Task<ObjectId> SaveFile(int id, string fileName, Stream source)
        {
            var result = await GetBucket().UploadFromStreamAsync(fileName, source, new GridFSUploadOptions()
            {
                Metadata = new BsonDocument { { "displayId", id } }
            });

            return result;
        }

        public async Task<int> DeleteFiles(int id)
        {
            int count = 0;

            var existing = await GetFiles(id);

            foreach (var item in existing.ToList())
            {
                bool deleted = await item.Delete();
                count += deleted ? 1 : 0;
            }

            return count;
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
    }
}