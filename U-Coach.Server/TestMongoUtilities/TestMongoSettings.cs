using MongoDB.Driver;
using PVDevelop.UCoach.Server.Configuration;

namespace TestMongoUtilities
{
    public sealed class TestMongoSettings : IConnectionStringProvider
    {
        public string Host { get; set; }
        public string DatabaseName { get; set; }

        public string ConnectionString
        {
            get
            {
                var builder = new MongoUrlBuilder();
                builder.Server = new MongoServerAddress(Host);
                builder.DatabaseName = DatabaseName;
                return builder.ToString();
            }
        }
    }
}
