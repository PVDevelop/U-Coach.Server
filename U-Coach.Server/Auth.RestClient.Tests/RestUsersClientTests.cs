using System;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.RestClient;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Auth.WebApi;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using Rhino.Mocks;
using StructureMap;
using StructureMap.AutoMocking;
using TestNUnit;

namespace Auth.RestClient.Tests
{
    [TestFixture]
    [Category(CategoryConst.REST)]
    public class RestUsersClientTests
    {
        [Test]
        public void Create_MockAuthService_CallsCreate()
        {
            // поднимаем web api сервер
            var connectionString = "http://localhost:5555";
            var selfHostConfiguraiton = new HttpSelfHostConfiguration(connectionString);
            selfHostConfiguraiton.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var userDto = new CreateUserDto()
            {
                Login = "l1",
                Password = "p1"
            };

            var expectedResult = "aaa";

            var mockUserService = MockRepository.GenerateMock<IUserService>();
            mockUserService.
                Expect(us=>us.Create(Arg<CreateUserDto>.Matches(dto=>
                    dto.Login == userDto.Login && 
                    dto.Password == userDto.Password))).
                Return(expectedResult);

            var container = new Container(x=> {
                x.For<IUserService>().Use(ctx => mockUserService);
            });

            selfHostConfiguraiton.Services.Replace(
                typeof(IHttpControllerActivator),
                new StructureMapControllerActivator(container));

            selfHostConfiguraiton.Services.Replace(
                typeof(IAssembliesResolver),
                new TestAssembliesResolver());

            string result;
            using (var server = new HttpSelfHostServer(selfHostConfiguraiton))
            {
                server.OpenAsync().Wait();

                var autoMocker = new RhinoAutoMocker<RestUsersClient>();

                var webConnString = MockRepository.GenerateStub<IConnectionStringProvider>();
                webConnString.Stub(sp => sp.ConnectionString).Return(connectionString);

                var clientFactory = new RestClientFactory(webConnString);
                autoMocker.Inject(typeof(IRestClientFactory), clientFactory);
                result = autoMocker.ClassUnderTest.Create(userDto);
            }

            mockUserService.VerifyAllExpectations();
            Assert.AreEqual(result, expectedResult);
        }
    }
}
