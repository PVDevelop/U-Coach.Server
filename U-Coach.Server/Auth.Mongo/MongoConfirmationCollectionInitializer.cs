using MongoDB.Driver;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Mongo;
using System;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    public class MongoConfirmationCollectionInitializer : IMongoInitializer
    {
        private readonly IConnectionStringProvider _settings;
        private readonly ILogger _logger = LoggerFactory.CreateLogger<MongoUserCollectionInitializer>();

        public MongoConfirmationCollectionInitializer(
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
            _logger.Debug(
                "Инициализирую коллекцию ключей подтверждения. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_settings));

            var collection = MongoHelper.GetCollection<MongoConfirmation>(_settings);

            var index = Builders<MongoConfirmation>.IndexKeys.Ascending(u => u.Key);
            var options = new CreateIndexOptions()
            {
                Name = MongoHelper.GetIndexName<MongoConfirmation>(nameof(MongoConfirmation.Key)),
                Unique = true
            };

            collection.Indexes.CreateOne(index, options);

            _logger.Debug("Инициализация коллекции ключей подтверждения прошла успешно.");
        }
    }
}
