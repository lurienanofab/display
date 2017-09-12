using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Configuration;

namespace Display.Models
{
    public abstract class RepositoryBase
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;

        public abstract string DatabaseName { get; }

        public RepositoryBase()
        {
            _client = new MongoClient(GetConnectionString());
            _database = _client.GetDatabase(DatabaseName);
        }

        protected string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MongoConnectionString"];
        }

        protected IGridFSBucket GetBucket()
        {
            return new GridFSBucket(_database);
        }
    }

    public abstract class RepositoryBase<T> : RepositoryBase
    {
        public abstract string CollectionName { get; }

        protected IMongoCollection<T> GetCollection()
        {
            return _database.GetCollection<T>(CollectionName);
        }
    }
}