using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Mongo;
using StructureMap;

namespace PVDevelop.UCoach.Server.Core.StructureMap
{
    public class SportsmanConfirmationRegistry : Registry
    {
        public SportsmanConfirmationRegistry()
        {
            For<IMongoConnectionSettings>().
                Use<MongoConnectionSettings>().
                Ctor<string>().
                Is("mongo_core").
                Named("settings_mongo_core");

            For<IMongoInitializer>().
                Use<MongoSportsmanConfirmationCollectionInitializer>().
                Ctor<IMongoConnectionSettings>("metaSettings").
                IsNamedInstance("settings_mongo_meta").
                Ctor<IMongoConnectionSettings>("contextSettings").
                IsNamedInstance("settings_mongo_core");

            For<IMongoRepository<MongoSportsmanConfirmation>>().
                Use<MongoRepository<MongoSportsmanConfirmation>>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_core");
        }
    }
}
