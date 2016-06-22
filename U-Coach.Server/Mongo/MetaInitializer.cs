using MongoDB.Driver;
using System;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MetaInitializer : IMongoInitializer
    {
        private readonly IMongoConnectionSettings _metaSettings;

        public MetaInitializer(IMongoConnectionSettings metaSettings)
        {
            if(metaSettings == null)
            {
                throw new ArgumentNullException("meatSettings");
            }
            _metaSettings = metaSettings;
        }

        public void Initialize()
        {
            var index = Builders<CollectionVersion>.IndexKeys.Ascending(c => c.Name);
            var options = new CreateIndexOptions()
            {
                Name = MongoHelper.GetIndexName<CollectionVersion>(nameof(CollectionVersion.Name)),
                Unique = true
            };

            var collection = MongoHelper.GetCollection<CollectionVersion>(_metaSettings);
            collection.Indexes.CreateOne(index, options);
        }
    }
}
