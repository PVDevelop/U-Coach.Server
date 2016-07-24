using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVDevelop.UCoach.Server.HttpGateway.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller
{
    public class UCoachAuthenticationController : ApiController
    {
        private readonly Role.Contract.IUsersClient _roleUsersClient;
        private readonly Auth.Contract.IUsersClient _authUsersClient;
        private readonly ITokenManager _tokenManager;

        public UCoachAuthenticationController(
            Auth.Contract.IUsersClient authUsersClient,
            Role.Contract.IUsersClient roleUsersClient,
            ITokenManager tokenManager)
        {
            if (authUsersClient == null)
            {
                throw new ArgumentNullException(nameof(authUsersClient));
            }
            if (roleUsersClient == null)
            {
                throw new ArgumentNullException(nameof(roleUsersClient));
            }
            if(tokenManager == null)
            {
                throw new ArgumentNullException(nameof(tokenManager));
            }

            _authUsersClient = authUsersClient;
            _roleUsersClient = roleUsersClient;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        [Route(Routes.UCOACH_TOKEN)]
        public IHttpActionResult GetToken(
            [FromUri(Name = "login")] string login,
            [FromUri(Name = "password")] string password)
        {
            var authToken = _authUsersClient.Logon(login, password);

            var userRegisterDto = new Role.Contract.AuthUserRegisterDto(authToken.Key, authToken.ExpiryDate);
            var roleTokenDto =
                _roleUsersClient.
                RegisterUser(AuthSystems.UCOACH_SYSTEM_NAME, authToken.UserId, userRegisterDto);

            // заполняем cookie
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _tokenManager.SetToken(this, response.Headers, roleTokenDto);

            return ResponseMessage(response);
        }
    }
}
