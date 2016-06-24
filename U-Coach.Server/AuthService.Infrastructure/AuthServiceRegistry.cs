using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService.Infrastructure
{
    public class AuthServiceRegistry : StructureMap.Registry
    {
        public AuthServiceRegistry()
        {
            For<IMongoInitializer>().
                Use<UserCollectionInitializer>().
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
