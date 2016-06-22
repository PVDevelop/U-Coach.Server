using System;
using MongoDB.Bson;
using MongoDB.Driver;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService
{
    /// <summary>
    /// Создает коллекцию пользователей и навешивает нужные ключи, индексы
    /// </summary>
    public class UserCollectionInitializer : IMongoInitializer
    {
        private readonly IMongoConnectionSettings _metaSettings;
        private readonly IMongoConnectionSettings _contextSettings;

        public UserCollectionInitializer(
            IMongoConnectionSettings metaSettings,
            IMongoConnectionSettings contextSettings)
        {
            if(metaSettings == null)
            {
                throw new ArgumentNullException("metaSettings");
            }
            if (contextSettings == null)
            {
                throw new ArgumentNullException("contextSettings");
            }
            _metaSettings = metaSettings;
            _contextSettings = contextSettings;
        }

        public void Initialize()
        {
            InitUserCollection();
            InitVersionCollection();
        }

        private void InitUserCollection()
        {
            var collection = MongoHelper.GetCollection<User>(_contextSettings);

            var index = Builders<User>.IndexKeys.Ascending(u => u.Login);
            var options = new CreateIndexOptions()
            {
                Name = MongoHelper.GetIndexName<User>(nameof(User.Login)),
                Unique = true
            };

            collection.Indexes.CreateOne(index, options);
        }

        private void InitVersionCollection()
        {
            var collection = MongoHelper.GetCollection<CollectionVersion>(_metaSettings);
            var collectionVersion = new CollectionVersion()
            {
                Name = MongoHelper.GetCollectionName<User>(),
                TargetVersion = MongoHelper.GetDataVersion<User>()
            };

            var options = new UpdateOptions()
            {
                IsUpsert = true
            };
            collection.ReplaceOne(cv => cv.Name == collectionVersion.Name, collectionVersion, options);
        }
    }
}
