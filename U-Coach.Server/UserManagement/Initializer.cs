using StructureMap;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PVDevelop.UCoach.Server.UserManagement.Executor;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Timing;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Core.Mail;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class Initializer
    {
        private static readonly object _sync = new object();
        private static Initializer _instance;
        private static readonly ILogger _logger = LoggerFactory.CreateLogger<Initializer>();

        public static Initializer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new Initializer();
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly Container _container;

        private Initializer()
        {
            _container = new Container(x =>
            {
                x.For<IUtcTimeProvider>().
                    Use<UtcTimeProvider>();

                x.For<IMongoConnectionSettings>().
                    Use<MongoConnectionSettings>().
                    Ctor<string>().
                    Is("mongo_meta").
                    Named("settings_mongo_meta");

                x.For<IMongoCollectionVersionValidator>().
                    Use<MongoCollectionVersionValidatorByClassAttribute>().
                    Ctor<IMongoConnectionSettings>().
                    IsNamedInstance("settings_mongo_meta");

                x.For<IMongoInitializer>().
                    Use<MongoMetaInitializer>().
                    Ctor<IMongoConnectionSettings>().
                    IsNamedInstance("settings_mongo_meta");

                x.For<IMongoConnectionSettings>().
                    Use<MongoConnectionSettings>().
                    Ctor<string>().
                    Is("mongo_auth").
                    Named("settings_mongo_auth");

                x.For<IMongoInitializer>().
                    Use<MongoUserCollectionInitializer>().
                    Ctor<IMongoConnectionSettings>("metaSettings").
                    IsNamedInstance("settings_mongo_meta").
                    Ctor<IMongoConnectionSettings>("contextSettings").
                    IsNamedInstance("settings_mongo_auth");

                x.For<IMongoRepository<MongoUser>>().
                    Use<MongoRepository<MongoUser>>().
                    Ctor<IMongoConnectionSettings>().
                    IsNamedInstance("settings_mongo_auth");

                x.For<IUserRepository>().
                    Use<MongoUserRepository>();

                x.For<IUserService>().
                    Use<UserService>();

                x.For<IUserFactory>().
                    Use<UserFactory>();

                x.For<IUsersClient>().
                    Use<CurrentDomainUsersClient>();

                x.For<IMongoConnectionSettings>().
                    Use<MongoConnectionSettings>().
                    Ctor<string>().
                    Is("mongo_core").
                    Named("settings_mongo_core");

                x.For<IMongoInitializer>().
                    Use<MongoSportsmanConfirmationCollectionInitializer>().
                    Ctor<IMongoConnectionSettings>("metaSettings").
                    IsNamedInstance("settings_mongo_meta").
                    Ctor<IMongoConnectionSettings>("contextSettings").
                    IsNamedInstance("settings_mongo_core");

                x.For<IMongoRepository<MongoSportsmanConfirmation>>().
                    Use<MongoRepository<MongoSportsmanConfirmation>>().
                    Ctor<IMongoConnectionSettings>().
                    IsNamedInstance("settings_mongo_core");

                x.For<ISportsmanConfirmationRepository>().
                    Use<MongoSportsmanConfirmationRepository>();

                x.For<ISportsmanConfirmationProducer>().
                    Use<EmailConfirmationProducer>();

                x.For<IEmailProducerSettings>().
                    Use<FakeEmailProducerSettings>();

                x.For<ISportsmanConfirmationService>().
                    Use<SportsmanConfirmationService>();

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

        public IEnumerable<T> GetAllInstances<T>()
        {
            try
            {
                return _container.GetAllInstances<T>();
            }
            catch(StructureMapConfigurationException ex)
            {
                _logger.Debug(ex, "Структура StructureMap: ", _container.WhatDoIHave());
                throw;
            }
        }
    }
}
