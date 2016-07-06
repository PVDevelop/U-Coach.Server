using System;
using System.Configuration;

namespace PVDevelop.UCoach.Server.Configuration
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string Name { get; private set; }

        /// <param name="name">Имя connection string в конфиге</param>
        public ConnectionStringProvider(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
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
