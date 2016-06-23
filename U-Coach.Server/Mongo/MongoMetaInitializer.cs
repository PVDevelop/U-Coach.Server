using MongoDB.Driver;
using System;
using PVDevelop.UCoach.Server.Logging;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MongoMetaInitializer : IMongoInitializer
    {
        private readonly IMongoConnectionSettings _metaSettings;
        private readonly ILogger _logger = LoggerFactory.CreateLogger<MongoMetaInitializer>();

        public MongoMetaInitializer(IMongoConnectionSettings metaSettings)
        {
            if(metaSettings == null)
            {
                throw new ArgumentNullException("meatSettings");
            }
            _metaSettings = metaSettings;
        }

        public void Initialize()
        {
            _logger.Debug(
                "Инициализирую метеданные. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_metaSettings));

            var index = Builders<CollectionVersion>.IndexKeys.Ascending(c => c.Name);
            var options = new CreateIndexOptions()
            {
                Name = MongoHelper.GetIndexName<CollectionVersion>(nameof(CollectionVersion.Name)),
                Unique = true
            };

            var collection = MongoHelper.GetCollection<CollectionVersion>(_metaSettings);
            collection.Indexes.CreateOne(index, options);

            _logger.Debug("Метаданные успешно инициализированы.");
        }
    }
}
