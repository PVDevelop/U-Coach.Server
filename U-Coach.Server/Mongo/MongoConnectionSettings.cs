using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MongoConnectionSettings : IMongoConnectionSettings
    {
        public string Name { get; private set; }

        /// <param name="name">Имя connection string в конфиге</param>
        public MongoConnectionSettings(string name)
        {
            Name = name;
        }

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[Name].ConnectionString;
            }
        }
    }
}
