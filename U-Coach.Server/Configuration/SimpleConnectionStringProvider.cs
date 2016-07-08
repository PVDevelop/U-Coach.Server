using System;

namespace PVDevelop.UCoach.Server.Configuration
{
    public class SimpleConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString { get; private set; }

        public SimpleConnectionStringProvider(string connectionString)
        {
            if(connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            ConnectionString = connectionString;
        }
    }
}
