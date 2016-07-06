using StructureMap;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PVDevelop.UCoach.Server.UserManagement.Executor;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Core.Mail;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Auth.RestClient;

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
                x.For<IConnectionStringProvider>().
                    Use<ConnectionStringProvider>().
                    Ctor<string>().
                    Is("mongo_meta").
                    Named("settings_mongo_meta");

                x.For<IMongoCollectionVersionValidator>().
                    Use<MongoCollectionVersionValidatorByClassAttribute>().
                    Ctor<IConnectionStringProvider>().
                    IsNamedInstance("settings_mongo_meta");

                x.For<IMongoInitializer>().
                    Use<MongoMetaInitializer>().
                    Ctor<IConnectionStringProvider>().
                    IsNamedInstance("settings_mongo_meta");

                x.For<IConnectionStringProvider>().
                    Use<ConnectionStringProvider>().
                    Named("conn_str_users_client").
                    Ctor<string>().
                    Is("rest_users");

                x.For<IUsersClient>().
                    Use<RestUsersClient>();

                x.For<IRestClientFactory>().
                    Use<RestClientFactory>().
                    Ctor<IConnectionStringProvider>().
                    IsNamedInstance("conn_str_users_client");

                x.For<IConnectionStringProvider>().
                    Use<ConnectionStringProvider>().
                    Ctor<string>().
                    Is("mongo_core").
                    Named("settings_mongo_core");

                x.For<IMongoInitializer>().
                    Use<MongoSportsmanConfirmationCollectionInitializer>().
                    Ctor<IConnectionStringProvider>("metaSettings").
                    IsNamedInstance("settings_mongo_meta").
                    Ctor<IConnectionStringProvider>("contextSettings").
                    IsNamedInstance("settings_mongo_core");

                x.For<IMongoRepository<MongoSportsmanConfirmation>>().
                    Use<MongoRepository<MongoSportsmanConfirmation>>().
                    Ctor<IConnectionStringProvider>().
                    IsNamedInstance("settings_mongo_core");

                x.For<ISportsmanConfirmationRepository>().
                    Use<MongoSportsmanConfirmationRepository>();

                x.For<ISportsmanConfirmationProducer>().
                    Use<EmailConfirmationProducer>();

                x.For<IEmailProducerSettings>().
                    Use<EmailProducerSettingsSection>();

                x.For<ISettingsProvider<IEmailProducerSettings>>().
                    Use<ConfigurationSectionSettingsProvider<IEmailProducerSettings>>().
                    Ctor<string>().
                    Is("emailProducerSettings");

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
