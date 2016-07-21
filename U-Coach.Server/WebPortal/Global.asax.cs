using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.WebApi;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.HttpGateway.WebApi;
using PVDevelop.UCoach.Server.RestClient;
using StructureMap;
using PVDevelop.UCoach.Server.Timing;

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
                x.For<IConnectionStringProvider>().Use<ConfigurationConnectionStringProvider>().Ctor<string>().Is("role");
                x.For<IRestClientFactory>().Use<RestClientFactory>();
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
