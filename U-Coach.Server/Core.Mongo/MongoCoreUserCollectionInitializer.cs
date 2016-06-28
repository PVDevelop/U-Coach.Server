using System;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Logging;
using MongoDB.Driver;

namespace PVDevelop.UCoach.Server.Core.Mongo
{
    public class MongoCoreUserCollectionInitializer : IMongoInitializer
    {
        private readonly IMongoConnectionSettings _metaSettings;
        private readonly IMongoConnectionSettings _contextSettings;
        private readonly ILogger _logger = LoggerFactory.CreateLogger<MongoCoreUserCollectionInitializer>();

        public MongoCoreUserCollectionInitializer(
            IMongoConnectionSettings metaSettings,
            IMongoConnectionSettings contextSettings)
        {
            if (metaSettings == null)
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
                "Инициализирую коллекцию пользователей ядра. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_contextSettings));

            var collection = MongoHelper.GetCollection<MongoCoreUser>(_contextSettings);

            var index = Builders<MongoCoreUser>.IndexKeys.Ascending(u => u.AuthSystem).Ascending(u=>u.AuthId);
            var indexName = MongoHelper.GetCompoundIndexName<MongoCoreUser>(
                nameof(MongoCoreUser.AuthSystem), 
                nameof(MongoCoreUser.AuthId));

            var options = new CreateIndexOptions()
            {
                Name = indexName,
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
                Name = MongoHelper.GetCollectionName<MongoCoreUser>(),
                TargetVersion = MongoHelper.GetDataVersion<MongoCoreUser>()
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
