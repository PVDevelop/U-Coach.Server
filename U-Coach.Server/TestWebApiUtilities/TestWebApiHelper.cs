using System;
using PVDevelop.UCoach.Server.Configuration;
using Rhino.Mocks;
using StructureMap;

namespace TestWebApiUtilities
{
    public static class TestWebApiHelper
    {
        public static T WithServer<T>(
            int port,
            Action<ConfigurationExpression> configureStructureMap,
            Func<TestWebApiSelfHost, T> act)
        {
            // act
            using (var server = new TestWebApiSelfHost(port, configureStructureMap))
            {
                var webConnString = MockRepository.GenerateStub<IConnectionStringProvider>();
                webConnString.Stub(sp => sp.ConnectionString).Return(server.ConnectionString);

                return act(server);
            }
        }
    }
}
