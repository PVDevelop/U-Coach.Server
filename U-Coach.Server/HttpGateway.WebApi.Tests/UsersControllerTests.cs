using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller;
using PVDevelop.UCoach.Server.Role.Contract;
using Rhino.Mocks;
using StructureMap.AutoMocking;
using TestComparisonUtilities;

namespace HttpGateway.WebApi.Tests
{
    [TestFixture]
    public class UsersControllerTests
    {
        [Test]
        public void GetUserInfo_MockUsersClient_CallsGetUserInfo()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UsersController>();

            var token = "some_token";
            var tokenManager = autoMocker.Get<ITokenManager>();
            tokenManager.
                Stub(t => t.TryGet(
                    Arg<ApiController>.Is.Same(autoMocker.ClassUnderTest),
                    out Arg<string>.Out(token).Dummy)).
                Return(true);

            var expectedUserInfoDto = new UserInfoDto("some_user_id");
            var usersClient = autoMocker.Get<IUsersClient>();
            usersClient.Expect(uc => uc.GetUserInfo(token)).Return(expectedUserInfoDto);

            // act
            var httpResult = autoMocker.ClassUnderTest.GetUserInfo();

            // assert
            usersClient.VerifyAllExpectations();

            Assert.IsInstanceOf<OkNegotiatedContentResult<UserInfoDto>>(httpResult);

            var userInfoDto = ((OkNegotiatedContentResult<UserInfoDto>)httpResult).Content;
            string comparison;
            Assert.IsTrue(new TestComparer().Compare(expectedUserInfoDto, userInfoDto, out comparison), comparison);
        }

        [Test]
        public void Logout_MockTokenManager_DeletesToken()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UsersController>();

            var tokenManager = autoMocker.Get<ITokenManager>();
            tokenManager.Expect(t => t.Delete(
                Arg<ApiController>.Is.Same(autoMocker.ClassUnderTest),
                Arg<HttpResponseHeaders>.Is.Anything));

            // act
            var httpResult = autoMocker.ClassUnderTest.Logout();

            // assert
            tokenManager.VerifyAllExpectations();
            Assert.IsInstanceOf<ResponseMessageResult>(httpResult);
            var responseMessageResult = (ResponseMessageResult)httpResult;
            Assert.AreEqual(HttpStatusCode.OK, responseMessageResult.Response.StatusCode);
        }
    }
}
