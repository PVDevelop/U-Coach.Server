using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.WebApi;
using StructureMap;

namespace MvcAuthrorization
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new ContainerBasedControllerFactory(CreateContainer()));
        }

        private IContainer CreateContainer()
        {
            return new Container(x =>
            {
                x.For<IConnectionStringProvider>().Use<ConfigurationConnectionStringProvider>().Ctor<string>().Is("portal");
                x.For<IActionResultBuilderFactory>().Use<ActionResultBuilderFactory>();
            });
        }
    }
}
