using System.Configuration;

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
