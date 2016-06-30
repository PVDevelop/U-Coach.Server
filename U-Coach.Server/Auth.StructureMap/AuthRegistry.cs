using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Mongo;
using StructureMap;

namespace PVDevelop.UCoach.Server.Auth.StructureMap
{ 
    public class AuthRegistry : Registry
    {
        public AuthRegistry()
        {
            For<IMongoConnectionSettings>().
                Use<MongoConnectionSettings>().
                Ctor<string>().
                Is("mongo_auth").
                Named("settings_mongo_auth");

            For<IMongoInitializer>().
                Use<MongoUserCollectionInitializer>().
                Ctor<IMongoConnectionSettings>("metaSettings").
                IsNamedInstance("settings_mongo_meta").
                Ctor<IMongoConnectionSettings>("contextSettings").
                IsNamedInstance("settings_mongo_auth");

            For<IMongoRepository<MongoUser>>().
                Use<MongoRepository<MongoUser>>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_auth");

            For<IUserRepository>().
                Use<MongoUserRepository>();

            For<IUserService>().Use<UserService>();

            For<IUserFactory>().Use<UserFactory>();
        }
    }
}
