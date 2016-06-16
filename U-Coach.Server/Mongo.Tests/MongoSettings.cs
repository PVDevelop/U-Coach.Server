using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    public sealed class MongoSettings : IMongoConnectionSettings
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
