using StructureMap;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace PVDevelop.UCoach.Server.Auth.IisWebApiHost
{
    public class StructureMapControllerActivator : IHttpControllerActivator
    {
        private readonly Container _container;

        public StructureMapControllerActivator(Container container)
        {
            if(container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            _container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request, 
            HttpControllerDescriptor controllerDescriptor, 
            Type controllerType)
        {
            return (IHttpController)_container.GetInstance(controllerType);
        }
    }
}