using Auth.IisWebApiHost;
using StructureMap;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace PVDevelop.UCoach.Server.Auth.IisWebApiHost
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new StructureMapControllerActivator(SetupContainer()));
        }

        private Container SetupContainer()
        {
            return new Container(x =>
            {
            });
        }
    }
}
