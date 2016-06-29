using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Mongo;
using StructureMap;

namespace PVDevelop.UCoach.Server.Core.StructureMap
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<IMongoConnectionSettings>().
                Use<MongoConnectionSettings>().
                Ctor<string>().
                Is("mongo_core").
                Named("settings_mongo_core");

            For<IMongoInitializer>().
                Use<MongoCoreUserCollectionInitializer>().
                Ctor<IMongoConnectionSettings>("metaSettings").
                IsNamedInstance("settings_mongo_meta").
                Ctor<IMongoConnectionSettings>("contextSettings").
                IsNamedInstance("settings_mongo_core");

            For<IMongoRepository<MongoCoreUser>>().
                Use<MongoRepository<MongoCoreUser>>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_core");
        }
    }
}
