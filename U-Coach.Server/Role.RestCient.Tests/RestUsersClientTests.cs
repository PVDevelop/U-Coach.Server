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
        public void GetUserInfo_MockUserService_CallsGetUserByToken()
        {
            // arrange
            var userService = MockRepository.GenerateMock<IUserService>();

            var userId = new UserId(Guid.NewGuid());
            var authUserId = new AuthUserId("system", "id");
            var tokenId = new TokenId("some_tokenId");

            var user = new User(userId, authUserId);

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
            Assert.AreEqual(userId.Id, userInfoDto.Id);
        }
    }
}
