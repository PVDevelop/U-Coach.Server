using System.Web.Http;
using System.Web.Http.Dispatcher;
using PVDevelop.UCoach.Server.Auth.WebApi;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.WebApi;
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
                x.For<IConnectionStringProvider>().Use<ConfigurationConnectionStringProvider>().Ctor<string>().Is("role");
                x.For<IActionResultBuilderFactory>().Use<ActionResultBuilderFactory>();
            });

            return _container;
        }
    }
}
