using System;
using PVDevelop.UCoach.Server.Configuration;

namespace TestMongoUtilities
{
    public class TestConfigurationMongoSettings : IConfiguration<TestMongoSettings>
    {
        public TestMongoSettings Value { get; private set; }

        public TestConfigurationMongoSettings(string host, string dbName)
        {
            Value = new TestMongoSettings()
            {
                Host = host,
                DatabaseName = dbName
            };
        }
    }
}
