using MongoDB.Driver;
using System;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    public static class TestMongoHelper
    {
        public static MongoSettings CreateSettings()
        {
            return new MongoSettings()
            {
                DatabaseName = string.Format("Test_{0}", Guid.NewGuid()),
                Host = "localhost"
            };
        }

        public static void WithDb(MongoSettings settings, Action<IMongoDatabase> callback)
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
