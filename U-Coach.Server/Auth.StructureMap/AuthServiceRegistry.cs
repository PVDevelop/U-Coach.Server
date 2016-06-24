using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Mongo;
using StructureMap;

namespace PVDevelop.UCoach.Server.Auth.StructureMap
{ 
    public class AuthServiceRegistry : Registry
    {
        public AuthServiceRegistry()
        {
            For<IMongoInitializer>().
                Use<MongoUserCollectionInitializer>().
                Ctor<IMongoConnectionSettings>("metaSettings").
                IsNamedInstance("settings_mongo_meta").
                Ctor<IMongoConnectionSettings>("contextSettings").
                IsNamedInstance("settings_mongo_context");

            For<IMongoRepository<MongoUser>>().
                Use<MongoRepository<MongoUser>>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_context");

            For<IUserRepository>().
                Use<MongoUserRepository>();

            For<IUserService>().Use<UserService>();
        }
    }
}
