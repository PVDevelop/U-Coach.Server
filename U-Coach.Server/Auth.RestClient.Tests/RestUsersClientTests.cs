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

        [Test]
        public void Create_MockAuthService_CallsCreate()
        {
            // arrange
            var userDto = new CreateUserDto()
            {
                Login = "l1",
                Password = "p1"
            };

            var expectedResult = new CreateUserResultDto()
            {
                Id = "SomeId"
            };

            var mockUserService = MockRepository.GenerateMock<IUserService>();
            mockUserService.
                Expect(us => us.Create(Arg<CreateUserDto>.Matches(dto =>
                      dto.Login == userDto.Login &&
                      dto.Password == userDto.Password))).
                Return(expectedResult);

            // act
            var result = WithServer(5000, mockUserService, client => client.Create(userDto));

            // assert
            mockUserService.VerifyAllExpectations();
            string comparison;
            Assert.IsTrue(new TestComparer().Compare(expectedResult, result, out comparison), comparison);
        }

        [Test]
        public void Logon_MockAuthService_CallsLogon()
        {
            // arrange
            var logonDto = new LogonUserDto()
            {
                Login = "l1",
                Password = "password"
            };

            var expectedResult = new LogonUserResultDto()
            {
                Token = "some_token"
            };

            var mockUserService = MockRepository.GenerateMock<IUserService>();
            mockUserService.
                Expect(us => us.Logon(Arg<LogonUserDto>.Matches(dto =>
                      dto.Login == logonDto.Login &&
                      dto.Password == logonDto.Password))).
                Return(expectedResult);

            // act
            var result = WithServer(5001, mockUserService, client => client.Logon(logonDto));

            // assert
            mockUserService.VerifyAllExpectations();
            string comparison;
            Assert.IsTrue(new TestComparer().Compare(expectedResult, result, out comparison), comparison);
        }

        [Test]
        public void Validate_MockAuthService_CallsValidate()
        {
            // arrange
            var tokenDto = new ValidateTokenDto()
            {
                Login = "l1",
                Token = "token"
            };

            var mockUserService = MockRepository.GenerateMock<IUserService>();
            mockUserService.
                Expect(us => us.ValidateToken(Arg<ValidateTokenDto>.Matches(dto =>
                      dto.Login == tokenDto.Login &&
                      dto.Token == tokenDto.Token)));

            // act
            WithServer(5002, mockUserService, client => client.ValidateToken(tokenDto));

            // assert
            mockUserService.VerifyAllExpectations();
        }
    }
}
