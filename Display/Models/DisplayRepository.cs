using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Display.Models
{
    public class DisplayRepository : RepositoryBase<DisplayModel>
    {
        private static readonly DisplayRepository _current;

        static DisplayRepository()
        {
            _current = new DisplayRepository();
        }

        public static DisplayRepository Current { get { return _current; } }

        public override string CollectionName
        {
            get { return "display"; }
        }

        public override string DatabaseName
        {
            get { return "display"; }
        }

        public async Task<IEnumerable<DisplayModel>> GetDisplays()
        {
            var builder = Builders<DisplayModel>.Filter;
            var filter = builder.Empty;
            var cursor = await GetCollection().FindAsync(filter);
            var result = await cursor.ToListAsync();
            return result;
        }

        public async Task<DisplayModel> GetDisplay(int displayId)
        {
            var builder = Builders<DisplayModel>.Filter;
            var filter = builder.Eq(x => x.DisplayID, displayId);
            var cursor = await GetCollection().FindAsync(filter);
            var display = await cursor.FirstOrDefaultAsync();

            if (display == null)
            {
                display = new DisplayModel()
                {
                    Id = ObjectId.GenerateNewId(),
                    DisplayID = displayId,
                    Files = new FileModel[] { },
                    LastUpdateUTC = DateTime.UtcNow
                };

                await GetCollection().InsertOneAsync(display);
            }

            return display;
        }

        public async Task<DisplayModel> AddFile(int displayId, FileModel file)
        {
            var filter = Builders<DisplayModel>.Filter
                .Where(x => x.DisplayID == displayId);

            var update = Builders<DisplayModel>.Update
                .AddToSet(x => x.Files, file)
                .Set(x => x.LastUpdateUTC, DateTime.UtcNow);

            var result = await GetCollection().FindOneAndUpdateAsync(filter, update);

            return result;
        }

        public async Task<DisplayModel> RemoveFile(int displayId, ObjectId contentId)
        {
            var filter = Builders<DisplayModel>.Filter
                .Where(x => x.DisplayID == displayId);

            var update = Builders<DisplayModel>.Update
                .PullFilter(x => x.Files, x => x.ContentId == contentId)
                .Set(x => x.LastUpdateUTC, DateTime.UtcNow);

            var result = await GetCollection().FindOneAndUpdateAsync(filter, update);

            return result;
        }

        public async Task<DisplayModel> SetFileActive(int displayId, ObjectId contentId, bool value)
        {
            var filter = Builders<DisplayModel>.Filter
                .Where(x => x.DisplayID == displayId && x.Files.Any(f => f.ContentId == contentId));

            var update = Builders<DisplayModel>.Update
                .Set(x => x.Files.ElementAt(-1).Active, value)
                .Set(x => x.LastUpdateUTC, DateTime.UtcNow);

            var result = await GetCollection().FindOneAndUpdateAsync(filter, update);

            return result;
        }
    }
}