using System;
using MongoDB.Bson;
using MongoDB.Driver;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    /// <summary>
    /// Создает коллекцию пользователей и навешивает нужные ключи, индексы
    /// </summary>
    public class MongoUserCollectionInitializer : IMongoInitializer
    {
        private readonly IConnectionStringProvider _settings;
        private readonly ILogger _logger = LoggerFactory.CreateLogger<MongoUserCollectionInitializer>();

        public MongoUserCollectionInitializer(
            IConnectionStringProvider settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _settings = settings;
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
                MongoHelper.SettingsToString(_settings));

            var collection = MongoHelper.GetCollection<MongoUser>(_settings);

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
                "Инициализирую метаданные пользователей. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_settings));

            var collection = MongoHelper.GetCollection<CollectionVersion>(_settings);
            var collectionVersion = new CollectionVersion(MongoHelper.GetCollectionName<MongoUser>())
            {
                TargetVersion = MongoHelper.GetDataVersion<MongoUser>()
            };

            var options = new UpdateOptions()
            {
                IsUpsert = true
            };

            if (collection.Find(cv => cv.TargetCollectionName == collectionVersion.TargetCollectionName).SingleOrDefault() == null)
            {
                collection.ReplaceOne(cv => cv.TargetCollectionName == collectionVersion.TargetCollectionName, collectionVersion, options);
            }

            _logger.Debug("Инициализация метаданных пользователей прошла успешно.");
        }
    }
}
