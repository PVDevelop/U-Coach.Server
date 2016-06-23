using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService.Infrastructure
{
    public class Registry : StructureMap.Registry
    {
        public Registry()
        {
            For<IUserService>().Use<UserService>();

            For<IMongoInitializer>().
                Use<UserCollectionInitializer>().
                Ctor<IMongoConnectionSettings>("metaSettings").
                IsNamedInstance("settings_mongo_meta").
                Ctor<IMongoConnectionSettings>("contextSettings").
                IsNamedInstance("settings_mongo_context");

            For<IMongoRepository<User>>().
                Use<MongoRepository<User>>().
                Ctor<IMongoConnectionSettings>().
                IsNamedInstance("settings_mongo_context");
        }
    }
}
