using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.Auth.WebApi;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Role.Mongo;
using PVDevelop.UCoach.Server.Role.Service;
using PVDevelop.UCoach.Server.Timing;
using StructureMap;

namespace Role.IisWebApiHost
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private Container _container;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new StructureMapControllerActivator(SetupContainer()));
        }

        private Container SetupContainer()
        {
            _container = new Container(x =>
            {
                x.For<IUserService>().Use<UserService>();
                x.For<IUserFactory>().Use<UserFactory>();
                x.For<IUserRepository>().Use<UserRepository>();
                x.For<IMongoRepository<MongoUser>>().Use<MongoRepository<MongoUser>>();
                x.For<IConnectionStringProvider>().Use<ConfigurationConnectionStringProvider>().Ctor<string>().Is("mongo");
                x.
                    For<ISettingsProvider<IFacebookOAuthSettings>>().
                    Use<ConfigurationSectionSettingsProvider<IFacebookOAuthSettings>>().
                    Ctor<string>().
                    Is("facebookSettings");
                x.For<IFacebookOAuthSettings>().Use<FacebookOAuthSettingsSection>();
                x.For<IUtcTimeProvider>().Use<UtcTimeProvider>();
            });

            return _container;
        }
    }
}
