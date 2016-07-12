using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.Auth.WebApi;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.RestClient;
using StructureMap;

namespace WebPortal
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
                x.For<IFacebookClient>().Use<FacebookClient>();
                x.For<IRestClientFactory>().Use<RestClientFactory>();
                x.For<IConnectionStringProvider>().Use<ConfigurationConnectionStringProvider>().Ctor<string>().Is("role");
            });

            return _container;
        }
    }
}
