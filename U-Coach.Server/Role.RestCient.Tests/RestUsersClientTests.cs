using NUnit.Framework;
using TestNUnit;
using TestWebApiUtilities;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Role.RestClient;
using StructureMap.AutoMocking;
using Rhino.Mocks;
using System;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;

namespace Role.RestCient.Tests
{
    [TestFixture]
    [Category(CategoryConst.REST)]
    public class RestUsersClientTests
    {
        private static T WithServer<T>(
            int port,
            IUserService userService,
            Func<RestUsersClient, T> callback)
        {
            return TestWebApiHelper.WithServer(
                port,
                x => x.For<IUserService>().Use(ctx => userService),
                server =>
                {
                    var autoMocker = new RhinoAutoMocker<RestUsersClient>();

                    var webConnString = autoMocker.Get<IConnectionStringProvider>();
                    webConnString.Stub(sp => sp.ConnectionString).Return(server.ConnectionString);

                    var clientFactory = new RestClientFactory(webConnString);
                    autoMocker.Inject(typeof(IRestClientFactory), clientFactory);

                    return callback(autoMocker.ClassUnderTest);
                });
        }

        [Test]
        public void RegisterUser_MockUserService_CallsRegisterUserToken()
        {
            // arrange
            var userService = MockRepository.GenerateMock<IUserService>();

            var userId = new UserId("system", "id");
            var authToken = new AuthSystemToken("token", DateTime.UtcNow);

            var token = new Token(new TokenId("tokenId"), userId, authToken, DateTime.UtcNow.AddDays(1));

            userService.
                Expect(us => 
                    us.RegisterUserToken(
                        Arg<UserId>.Is.Equal(userId), 
                        Arg<AuthSystemToken>.Matches(t => t.Token == authToken.Token && t.Expiration == authToken.Expiration))).
                Return(token);

            // act
            var tokenDto = WithServer(6000, userService, client =>
            {
                var authUserRegisterDto = new AuthUserRegisterDto(authToken.Token, authToken.Expiration);
                return client.RegisterUser(userId.AuthSystemName, userId.AuthId, authUserRegisterDto);
            });

            // assert
            userService.VerifyAllExpectations();
            Assert.AreEqual(token.Id.Token, tokenDto.Token);
            Assert.AreEqual(token.Expiration, tokenDto.Expiration);
        }

        [Test]
        public void GetUserInfo_MockUserService_CallsGetUserByToken()
        {
            // arrange
            var userService = MockRepository.GenerateMock<IUserService>();

            var userId = new UserId("system", "id");
            var tokenId = new TokenId("some_tokenId");

            var user = new User(userId);

            userService.
                Expect(us => us.GetUserByToken(tokenId)).
                Return(user);

            // act
            var userInfoDto = WithServer(6001, userService, client =>
            {
                return client.GetUserInfo(tokenId.Token);
            });

            // assert
            userService.VerifyAllExpectations();
            var expectedUserId = string.Format("{0}.{1}", user.Id.AuthSystemName, user.Id.AuthId);
            Assert.AreEqual(expectedUserId, userInfoDto.Id);
        }
    }
}
