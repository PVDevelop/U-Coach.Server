using System;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Logging;
using MongoDB.Driver;
using PVDevelop.UCoach.Server.Configuration;

namespace PVDevelop.UCoach.Server.Core.Mongo
{
    public class MongoSportsmanConfirmationCollectionInitializer : IMongoInitializer
    {
        private readonly IConnectionStringProvider _settings;
        private readonly ILogger _logger = LoggerFactory.CreateLogger<MongoSportsmanConfirmationCollectionInitializer>();

        public MongoSportsmanConfirmationCollectionInitializer(
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
                "Инициализирую коллекцию подтверждения спортсменов. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_settings));

            var collection = MongoHelper.GetCollection<MongoSportsmanConfirmation>(_settings);

            var index = Builders<MongoSportsmanConfirmation>.IndexKeys.Ascending(u => u.AuthSystem).Ascending(u=>u.AuthUserId);
            var indexName = MongoHelper.GetCompoundIndexName<MongoSportsmanConfirmation>(
                nameof(MongoSportsmanConfirmation.AuthSystem),
                nameof(MongoSportsmanConfirmation.AuthUserId),
                nameof(MongoSportsmanConfirmation.ConfirmationKey));

            var options = new CreateIndexOptions()
            {
                Name = indexName,
                Unique = true
            };

            collection.Indexes.CreateOne(index, options);

            _logger.Debug("Инициализация коллекции подтверждения спортсменов прошла успешно.");
        }

        private void InitVersionCollection()
        {
            _logger.Debug(
                "Инициализирую метаданные подтверждения спортсменов. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_settings));

            var collection = MongoHelper.GetCollection<CollectionVersion>(_settings);
            var collectionVersion = new CollectionVersion(MongoHelper.GetCollectionName<MongoSportsmanConfirmation>())
            {
                TargetVersion = MongoHelper.GetDataVersion<MongoSportsmanConfirmation>()
            };

            var options = new UpdateOptions()
            {
                IsUpsert = true
            };

            if (collection.Find(cv => cv.TargetCollectionName == collectionVersion.TargetCollectionName).SingleOrDefault() == null)
            {
                collection.ReplaceOne(cv => cv.TargetCollectionName == collectionVersion.TargetCollectionName, collectionVersion, options);
            }

            _logger.Debug("Инициализация метаданных подтверждения спортсменов прошла успешно.");
        }
    }
}
