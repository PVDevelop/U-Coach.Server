﻿using Auth.IisWebApiHost;
using StructureMap;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.Timing;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Auth.Domain;
using System;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Auth.WebApi;
using PVDevelop.UCoach.Server.Auth.Mail;
using PVDevelop.UCoach.Server.WebApi;

namespace PVDevelop.UCoach.Server.Auth.IisWebApiHost
{
    public class WebApiApplication : HttpApplication
    {
        private Container _container;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new StructureMapControllerActivator(SetupContainer()));

            InitializeSystem();
        }

        private Container SetupContainer()
        {
            _container = new Container(x =>
            {
                ConfigureTiming(x);
                ConfigureMongo(x);
                ConfigureUserService(x);
            });

            return _container;
        }

        private void ConfigureTiming(ConfigurationExpression x)
        {
            x.For<IUtcTimeProvider>().
                Use<UtcTimeProvider>();
        }

        private void ConfigureMongo(ConfigurationExpression x)
        {
            x.For<IMongoCollectionVersionValidator>().
                Use<MongoCollectionVersionValidatorByClassAttribute>();

            x.For<IConnectionStringProvider>().
                Use<ConfigurationConnectionStringProvider>().
                Ctor<string>().
                Is("mongo");

            x.For<IMongoInitializer>().
                Use<MongoUserCollectionInitializer>();

            x.For<IMongoInitializer>().
                Use<MongoTokenCollectionInitializer>();

            x.For<IMongoInitializer>().
                Use<MongoConfirmationCollectionInitializer>();

            x.For<IMongoRepository<MongoUser>>().
                Use<MongoRepository<MongoUser>>();

            x.For<IMongoRepository<MongoToken>>().
                Use<MongoRepository<MongoToken>>();

            x.For<IMongoRepository<MongoConfirmation>>().
                Use<MongoRepository<MongoConfirmation>>();

            x.For<IUserRepository>().
                Use<MongoUserRepository>();

            x.For<ITokenRepository>().
                Use<MongoTokenRepository>();

            x.For<IConfirmationRepository>().
                Use<MongoConfirmationRepository>();
        }

        private void ConfigureUserService(ConfigurationExpression x)
        {
            x.For<IUserService>().
                Use<UserService>();

            x.For<IUserValidator>().
                Use<UserValidator>();

            x.For<IKeyGeneratorService>().
                Use<KeyGeneratorService>();

            x.For<IUtcTimeProvider>().
                Use<UtcTimeProvider>();

            x.For<IConfirmationProducer>().
                Use<EmailConfirmationProducer>();

            x.For<ISettingsProvider<IEmailProducerSettings>>().
                Use<ConfigurationSectionSettingsProvider<IEmailProducerSettings>>().
                Ctor<string>().
                Is("emailProducerSettings");
        }

        private void InitializeSystem()
        {
            var mongoInitializers = _container.GetAllInstances<IMongoInitializer>();
            foreach(var initializer in mongoInitializers)
            {
                initializer.Initialize();
            }
        }
    }
}
