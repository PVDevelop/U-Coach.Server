using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using StructureMap;

namespace MvcAuthrorization
{
    public class ContainerBasedControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;
        private readonly Assembly _myAssembly;

        public ContainerBasedControllerFactory(IContainer container)
        {
            if(container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
            _myAssembly = Assembly.GetCallingAssembly();
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var controllerTypeName = string.Format("{0}Controller", controllerName);
            var controllerType =
                _myAssembly.
                GetTypes().
                Where(t => t.IsSubclassOf(typeof(Controller))).
                Single(t => t.Name.Equals(controllerTypeName, StringComparison.OrdinalIgnoreCase));

            return (IController)_container.GetInstance(controllerType);
        }
    }
}