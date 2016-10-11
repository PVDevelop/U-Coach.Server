using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public class UsersController : ApiController
    {
        private readonly IUsersClient _usersClient;
        private readonly ITokensClient _tokensClient;
        private readonly ITokenManager _tokenManager;

        public UsersController(
            IUsersClient usersClient,
            ITokensClient tokensClient,
            ITokenManager tokenManager)
        {
            if (usersClient == null)
            {
                throw new ArgumentNullException(nameof(usersClient));
            }
            if(tokenManager == null)
            {
                throw new ArgumentNullException(nameof(tokenManager));
            }
            if(tokensClient == null)
            {
                throw new ArgumentNullException(nameof(tokensClient));
            }

            _usersClient = usersClient;
            _tokenManager = tokenManager;
            _tokensClient = tokensClient;
        }

        [HttpGet]
        [Route(Contract.Routes.USER_INFO)]
        public IHttpActionResult GetUserInfo()
        {
            string token;
            if(!_tokenManager.TryGet(this, out token))
            {
                throw new ApplicationException("Empty token in cookies");
            }

            var userInfo = _usersClient.GetUserInfo(token);
            return Ok(userInfo);
        }

        [HttpDelete]
        [Route(Contract.Routes.LOGOUT)]
        public IHttpActionResult Logout()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            string token;
            if (_tokenManager.TryGet(this, out token))
            {
                _tokensClient.Delete(token);
                _tokenManager.Delete(this, response.Headers);
            }

            return ResponseMessage(response);
        }
    }
}
