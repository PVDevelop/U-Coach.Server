using PVDevelop.UCoach.Server.AuthService;
using PVDevelop.UCoach.Server.Mongo;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class ExecutorContainer
    {
        private static readonly object _sync = new object();
        private static ExecutorContainer _instance;

        public static ExecutorContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new ExecutorContainer();
                        }
                    }
                }
                return _instance;
            }
        }

        public Container Container { get; private set; }

        private ExecutorContainer()
        {
            Container = new Container(x =>
            {
                x.
                    For<IMongoConnectionSettings>().
                    Use<MongoConnectionSettings>().
                    Ctor<string>().
                    Is("mongo_meta").
                    Named("settings_mongo_meta");

                x.
                    For<IMongoCollectionVersionValidator>().
                    Use<MongoCollectionVersionValidatorByClassAttribute>().
                    Ctor<IMongoConnectionSettings>().
                    IsNamedInstance("settings_mongo_meta");

                x.
                    For<IMongoConnectionSettings>().
                    Use<MongoConnectionSettings>().
                    Ctor<string>().
                    Is("mongo_context").
                    Named("settings_mongo_context");

                x.
                    For<IMongoRepository<User>>().
                    Use<MongoRepository<User>>().
                    Ctor<IMongoConnectionSettings>().
                    IsNamedInstance("settings_mongo_context");

                x.
                    For<IUserService>().
                    Use<UserService>();

                x.
                    For<IMongoInitializer>().
                    Use<MetaInitializer>().
                    Ctor<IMongoConnectionSettings>().
                    IsNamedInstance("settings_mongo_meta");

                x.
                    For<IMongoInitializer>().
                    Use<UserCollectionInitializer>().
                    Ctor<IMongoConnectionSettings>("metaSettings").
                    IsNamedInstance("settings_mongo_meta").
                    Ctor<IMongoConnectionSettings>("contextSettings").
                    IsNamedInstance("settings_mongo_context");

                foreach (var type in 
                    Assembly.
                    GetExecutingAssembly().
                    GetTypes().
                    Where(t => t.GetInterface(typeof(IExecutor).Name) != null))
                {
                    x.For(typeof(IExecutor)).Use(type);
                }
            });
        }
    }
}
