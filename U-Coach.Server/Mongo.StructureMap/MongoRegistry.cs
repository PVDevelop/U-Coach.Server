using StructureMap;

namespace PVDevelop.UCoach.Server.Mongo.StructureMap
{
    public class MongoRegistry : Registry
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

            For<IMongoInitializer>().
                Use<MongoMetaInitializer>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_meta");
        }
    }
}
