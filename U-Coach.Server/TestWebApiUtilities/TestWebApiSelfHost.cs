using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using PVDevelop.UCoach.Server.WebApi;
using StructureMap;

namespace TestWebApiUtilities
{
    public class TestWebApiSelfHost : IDisposable
    {
        public string ConnectionString { get; private set; }

        private HttpSelfHostServer _server;

        public TestWebApiSelfHost(
            int port,
            Action<ConfigurationExpression> containerConfigurationExpression)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Port = port;
            uriBuilder.Host = "localhost";

            ConnectionString = uriBuilder.ToString();

            var selfHostConfiguraiton = new HttpSelfHostConfiguration(uriBuilder.Uri);
            selfHostConfiguraiton.MapHttpAttributeRoutes();
            selfHostConfiguraiton.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new Container(containerConfigurationExpression);

            selfHostConfiguraiton.Services.Replace(
                typeof(IHttpControllerActivator),
                new StructureMapControllerActivator(container));

            selfHostConfiguraiton.Services.Replace(
                typeof(IAssembliesResolver),
                new TestAssembliesResolver());

            _server = new HttpSelfHostServer(selfHostConfiguraiton);
            _server.OpenAsync().Wait();
        }

        public void Dispose()
        {
            if(_server != null)
            {
                _server.Dispose();
                _server = null;
            }
        }
    }
}
