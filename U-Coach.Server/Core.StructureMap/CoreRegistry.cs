using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Mongo;
using StructureMap;

namespace PVDevelop.UCoach.Server.Core.StructureMap
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<IMongoInitializer>().
                Use<MongoCoreUserCollectionInitializer>().
                Ctor<IMongoConnectionSettings>("metaSettings").
                IsNamedInstance("settings_mongo_meta").
                Ctor<IMongoConnectionSettings>("contextSettings").
                IsNamedInstance("settings_mongo_context");

            For<IMongoRepository<MongoCoreUser>>().
                Use<MongoRepository<MongoCoreUser>>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_context");

            //For<ICoreUserRepository>().
            //    Use<MongoCoreUserRepository>();

            //For<IUserService>().Use<UserService>();
        }
    }
}
