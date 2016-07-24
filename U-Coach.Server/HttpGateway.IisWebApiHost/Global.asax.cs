using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.WebApi;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using StructureMap;
using PVDevelop.UCoach.Server.Timing;
using PVDevelop.UCoach.Server.HttpGateway.WebApi.Settings;
using PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller;

namespace PVDevelop.UCoach.Server.HttpGateway.IisWebApiHost
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
                x.
                    For<IConnectionStringProvider>().
                    Use<ConfigurationConnectionStringProvider>().
                    Ctor<string>().
                    Is("role").
                    Named("role_settings");

                x.
                    For<IConnectionStringProvider>().
                    Use<ConfigurationConnectionStringProvider>().
                    Ctor<string>().
                    Is("auth").
                    Named("auth_settings");

                x.
                    For<IRestClientFactory>().
                    Use<RestClientFactory>().
                    Ctor<IConnectionStringProvider>().
                    IsNamedInstance("role_settings").
                    Named("role_rest_client_factory");

                x.
                    For<IRestClientFactory>().
                    Use<RestClientFactory>().
                    Ctor<IConnectionStringProvider>().
                    IsNamedInstance("auth_settings").
                    Named("auth_rest_client_factory");

                x.
                    For<Auth.Contract.IUsersClient>().
                    Use<Auth.RestClient.RestUsersClient>().
                    Ctor<IRestClientFactory>().
                    IsNamedInstance("auth_rest_client_factory");

                x.For<Role.Contract.IUsersClient>().
                    Use<Role.RestClient.RestUsersClient>().
                    Ctor<IRestClientFactory>().
                    IsNamedInstance("role_rest_client_factory");

                x.
                    For<ISettingsProvider<IFacebookOAuthSettings>>().
                    Use<ConfigurationSectionSettingsProvider<IFacebookOAuthSettings>>().
                    Ctor<string>().
                    Is("facebookSettings");

                x.For<IFacebookOAuthSettings>().Use<FacebookOAuthSettingsSection>();
                x.For<IUtcTimeProvider>().Use<UtcTimeProvider>();
                x.For<ITokenManager>().Use<CookiesTokenManager>();
            });

            return _container;
        }
    }
}
