using System;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using PVDevelop.UCoach.Server.HttpGateway.Contract;
using PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller;
using PVDevelop.UCoach.Server.Role.Contract;
using Rhino.Mocks;
using StructureMap.AutoMocking;
using TestComparisonUtilities;

namespace HttpGateway.WebApi.Tests
{
    [TestFixture]
    public class UCoachAuthenticationControllerTests
    {
        [Test]
        public void GetToken_MockClientsAndTokenManager_CallsRequiredMethods()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UCoachAuthenticationController>();

            var authUsersClient = autoMocker.Get<PVDevelop.UCoach.Server.Auth.Contract.IUsersClient>();
            var token = new PVDevelop.UCoach.Server.Auth.Domain.Token()
            {
                Key = "some_token",
                ExpiryDate = DateTime.UtcNow,
                UserId = "some_user"
            };
            authUsersClient.Expect(uc => uc.Logon("some_login", "some_password")).Return(token);

            var tokenDto = new TokenDto("my_new_token", DateTime.UtcNow);
            var roleUsersClient = autoMocker.Get<IUsersClient>();
            roleUsersClient.
                Expect(uc => uc.RegisterUser(
                    Arg<string>.Is.Equal(AuthSystems.UCOACH_SYSTEM_NAME),
                    Arg<string>.Is.Equal(token.UserId),
                    Arg<AuthUserRegisterDto>.Matches(dto => dto.Token == token.Key && dto.Expiration == token.ExpiryDate))).
                Return(tokenDto);

            var tokenManager = autoMocker.Get<ITokenManager>();
            string comparison;
            tokenManager.Expect(tm => tm.SetToken(
                Arg<ApiController>.Is.Same(autoMocker.ClassUnderTest),
                Arg<HttpResponseHeaders>.Is.Anything,
                Arg<TokenDto>.Matches(t => new TestComparer().Compare(tokenDto, t, out comparison))));

            // act
            var httpResult = autoMocker.ClassUnderTest.GetToken("some_login", "some_password");

            // assert
            authUsersClient.VerifyAllExpectations();
            roleUsersClient.VerifyAllExpectations();
            tokenManager.VerifyAllExpectations();

            Assert.IsInstanceOf<ResponseMessageResult>(httpResult);
            var responseMessageResult = (ResponseMessageResult)httpResult;
            Assert.AreEqual(HttpStatusCode.OK, responseMessageResult.Response.StatusCode);
        }
    }
}
