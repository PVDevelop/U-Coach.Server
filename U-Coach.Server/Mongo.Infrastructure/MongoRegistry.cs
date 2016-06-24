using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo.Infrastructure
{
    public class MongoRegistry : StructureMap.Registry
    {
        public MongoRegistry()
        {
            For<IMongoConnectionSettings>().
                Use<MongoConnectionSettings>().
                Ctor<string>().
                Is("mongo_meta").
                Named("settings_mongo_meta");

            For<IMongoCollectionVersionValidator>().
                Use<MongoCollectionVersionValidatorByClassAttribute>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_meta");

            For<IMongoConnectionSettings>().
                Use<MongoConnectionSettings>().
                Ctor<string>().
                Is("mongo_context").
                Named("settings_mongo_context");

            For<IMongoInitializer>().
                Use<MongoMetaInitializer>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_meta");
        }
    }
}
