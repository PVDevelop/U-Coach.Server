using MongoDB.Driver;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    public class MongoTokenCollectionInitializer : IMongoInitializer
    {
        private readonly IConnectionStringProvider _settings;
        private readonly ILogger _logger = LoggerFactory.CreateLogger<MongoUserCollectionInitializer>();

        public MongoTokenCollectionInitializer(
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
                "Инициализирую коллекцию токенов. Параметры подключения: {0}.",
                MongoHelper.SettingsToString(_settings));

            var collection = MongoHelper.GetCollection<MongoToken>(_settings);

            var index = Builders<MongoToken>.IndexKeys.Ascending(u => u.Key);
            var options = new CreateIndexOptions()
            {
                Name = MongoHelper.GetIndexName<MongoToken>(nameof(MongoToken.Key)),
                Unique = true
            };

            collection.Indexes.CreateOne(index, options);

            _logger.Debug("Инициализация коллекции токенов прошла успешно.");
        }
    }
}
