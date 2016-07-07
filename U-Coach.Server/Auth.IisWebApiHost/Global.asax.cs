using Auth.IisWebApiHost;
using StructureMap;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.Timing;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Auth.Domain;
using System;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Auth.WebApi;

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
                Use<ConnectionStringProvider>().
                Ctor<string>().
                Is("mongo");

            x.For<IMongoInitializer>().
                Use<MongoUserCollectionInitializer>();

            x.For<IMongoRepository<MongoUser>>().
                Use<MongoRepository<MongoUser>>();

            x.For<IUserRepository>().
                Use<MongoUserRepository>();
        }

        private void ConfigureUserService(ConfigurationExpression x)
        {
            x.For<IUserService>().
                Use<UserService>();

            x.For<IUserFactory>().
                Use<UserFactory>();
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
