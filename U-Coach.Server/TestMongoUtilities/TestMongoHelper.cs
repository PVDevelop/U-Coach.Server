using MongoDB.Driver;
using System;

namespace TestMongoUtilities
{
    public static class TestMongoHelper
    {
        public static TestMongoSettings CreateSettings()
        {
            return new TestMongoSettings()
            {
                DatabaseName = string.Format("Test_{0}", Guid.NewGuid()),
                Host = "localhost"
            };
        }

        public static void WithDb(TestMongoSettings settings, Action<IMongoDatabase> callback)
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
