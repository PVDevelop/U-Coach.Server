using System;
using MongoDB.Bson;
using MongoDB.Driver;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    /// <summary>
    /// Создает коллекцию пользователей и навешивает нужные ключи, индексы
    /// </summary>
    public class MongoUserCollectionInitializer : IMongoInitializer
    {
        private readonly IMongoConnectionSettings _metaSettings;
        private readonly IMongoConnectionSettings _contextSettings;
        private readonly ILogger _logger = LoggerFactory.CreateLogger<MongoUserCollectionInitializer>();

        public MongoUserCollectionInitializer(
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
            _logger.Debug(
                "Инициализирую коллекцию пользователей. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_contextSettings));

            var collection = MongoHelper.GetCollection<MongoUser>(_contextSettings);

            var index = Builders<MongoUser>.IndexKeys.Ascending(u => u.Login);
            var options = new CreateIndexOptions()
            {
                Name = MongoHelper.GetIndexName<MongoUser>(nameof(MongoUser.Login)),
                Unique = true
            };

            collection.Indexes.CreateOne(index, options);

            _logger.Debug("Инициализация коллекции пользователей прошла успешно.");
        }

        private void InitVersionCollection()
        {
            _logger.Debug(
                "Инициализирую метаданные пользователей. Параметры подключения meta: {0}, context: {1}.",
                MongoHelper.SettingsToString(_metaSettings),
                MongoHelper.SettingsToString(_contextSettings));

            var collection = MongoHelper.GetCollection<CollectionVersion>(_metaSettings);
            var collectionVersion = new CollectionVersion()
            {
                Name = MongoHelper.GetCollectionName<MongoUser>(),
                TargetVersion = MongoHelper.GetDataVersion<MongoUser>()
            };

            var options = new UpdateOptions()
            {
                IsUpsert = true
            };
            collection.ReplaceOne(cv => cv.Name == collectionVersion.Name, collectionVersion, options);

            _logger.Debug("Инициализация метаданных пользователей прошла успешно.");
        }
    }
}
