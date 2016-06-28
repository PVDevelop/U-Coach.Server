using Auth.IisWebApiHost;
using PVDevelop.UCoach.Server.Auth.AutoMapper;
using PVDevelop.UCoach.Server.Auth.StructureMap;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Mongo.StructureMap;
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
                x.AddRegistry<AuthRegistry>();
                x.AddRegistry<MongoRegistry>();
                x.For<IMapper>().Add(() => new MapperImpl(cfg => cfg.AddProfile<UserProfile>()));
            });
        }
    }
}
