using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace Auth.RestClient.Tests
{
    public class TestAssembliesResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            return new[] 
            {
                Assembly.Load("PVDevelop.UCoach.Server.Auth.WebApi")
            };
        }
    }
}
