using MongoDB.Driver;
using System;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    public static class MongoHelper
    {
        public static IMongoConnectionSettings CreateSettings(string collectionName)
        {
            return new MongoSettings()
            {
                CollectionName = collectionName,
                ConnectionString = "mongodb://localhost",
                DatabaseName = string.Format("Test_{0}", Guid.NewGuid())
            };
        }

        public static void WithDb(IMongoConnectionSettings settings, Action<IMongoDatabase> callback)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);

            try
            {
                callback(db);
            }
            finally
            {
                client.DropDatabase(settings.DatabaseName);
            }
        }
    }
}
