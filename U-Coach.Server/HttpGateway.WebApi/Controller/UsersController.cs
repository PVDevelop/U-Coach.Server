using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller
{
    public class UsersController : ApiController
    {
        private readonly IUsersClient _usersClient;
        private readonly ITokenManager _tokenManager;

        public UsersController(
            IUsersClient usersClient,
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

            _usersClient = usersClient;
            _tokenManager = tokenManager;
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
            _tokenManager.Delete(this, response.Headers);
            return ResponseMessage(response);
        }
    }
}
