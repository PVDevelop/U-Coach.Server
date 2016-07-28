using System;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.RestClient;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using Rhino.Mocks;
using StructureMap.AutoMocking;
using TestComparisonUtilities;
using TestNUnit;
using TestWebApiUtilities;
using PVDevelop.UCoach.Server.Auth.Domain;
using Auth.Domain.Tests;

namespace Auth.RestClient.Tests
{
    [TestFixture]
    [Category(CategoryConst.REST)]
    public class RestUsersClientTests
    {
        private static void WithServer(
            int port,
            IUserService userService,
            Action<RestUsersClient> callback)
        {
            WithServer<object>(port, userService, c =>
            {
                callback(c);
                return null;
            });
        }

        private static T WithServer<T>(
            int port,
            IUserService userService,
            Func<RestUsersClient, T> callback)
        {
            // act
            using (var server = new TestWebApiSelfHost(port, x => {
                x.For<IUserService>().Use(ctx => userService);
            }))
            {
                var autoMocker = new RhinoAutoMocker<RestUsersClient>();

                var webConnString = MockRepository.GenerateStub<IConnectionStringProvider>();
                webConnString.Stub(sp => sp.ConnectionString).Return(server.ConnectionString);

                var clientFactory = new RestClientFactory(webConnString);
                autoMocker.Inject(typeof(IRestClientFactory), clientFactory);

                return callback(autoMocker.ClassUnderTest);
            }
        }

#warning не проходит
        [Test]
        public void Create_MockAuthService_CallsCreate()
        {
            var mockUserService = MockRepository.GenerateMock<IUserService>();
            mockUserService.
                Expect(us => us.CreateUser("l1", "p1")).
                Return(new Token("11", "SomeId", new TestUtcTimeProvider()));

            // act
            var result = WithServer(5000, mockUserService, client => client.Create(new UserDto("l1", "p1")));

            // assert
            mockUserService.VerifyAllExpectations();
            Assert.Equals("SomeId", result.Key);
        }

#warning не проходит
        [Test]
        public void Logon_MockAuthService_CallsLogon()
        {
            var mockUserService = MockRepository.GenerateMock<IUserService>();
            mockUserService.
                Expect(us => us.Logon("l1", "password")).
                Return(new Token("l1", "some_token", new TestUtcTimeProvider()));

            // act
            var result = WithServer(5001, mockUserService, client => client.Logon("l1", new PasswordDto("password")));

            // assert
            mockUserService.VerifyAllExpectations();
            Assert.Equals("some_token", result.Key);
        }

        [Test]
        public void Validate_MockAuthService_CallsValidate()
        {
            var mockUserService = MockRepository.GenerateMock<IUserService>();
            mockUserService.
                Expect(us => us.ValidateToken("token"));

            // act
            WithServer(5002, mockUserService, client => client.ValidateToken(new TokenDto("token")));

            // assert
            mockUserService.VerifyAllExpectations();
        }
    }
}
