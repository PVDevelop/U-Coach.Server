using PVDevelop.UCoach.Server.AuthService.Host;
using PVDevelop.UCoach.Server.AuthService.Infrastructure;
using PVDevelop.UCoach.Server.Mongo.Infrastructure;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace AuthService.Host
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
            return new Container(x=> 
            {
                x.AddRegistry<AuthServiceRegistry>();
                x.AddRegistry<MongoRegistry>();
            });
        }
    }
}
