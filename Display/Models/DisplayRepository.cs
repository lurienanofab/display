using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Display.Models
{
    public class DisplayRepository : RepositoryBase<BsonDocument>
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

        public async Task<DisplayModel> GetDisplay(int displayId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(x => x["DisplayID"], displayId);
            var bdoc = await GetCollection().Find(filter).FirstOrDefaultAsync();

            if (bdoc != null)
            {
                return new DisplayModel()
                {
                    DisplayID = bdoc["DisplayID"].AsInt32,
                    LastUpdateUTC = bdoc["LastUpdate"].ToUniversalTime()
                };
            }
            else
            {
                return new DisplayModel()
                {
                    DisplayID = displayId,
                    LastUpdateUTC = DateTime.UtcNow
                };
            }
        }

        public async Task Update(int displayId)
        {
            var display = await GetDisplay(displayId);

            var filter = Builders<BsonDocument>.Filter
                .Eq(x => x["DisplayID"], display.DisplayID);

            var update = Builders<BsonDocument>.Update
                .Set(x => x["DisplayID"], display.DisplayID)
                .Set(x => x["LastUpdate"], DateTime.UtcNow);

            await GetCollection().UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true });
        }
    }
}