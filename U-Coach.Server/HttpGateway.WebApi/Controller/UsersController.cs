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
        private readonly IUtcTimeProvider _utcTimeProvider;

        public UsersController(
            IUsersClient usersClient,
            IUtcTimeProvider utcTimeProvider)
        {
            if (usersClient == null)
            {
                throw new ArgumentNullException(nameof(usersClient));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _usersClient = usersClient;
            _utcTimeProvider = utcTimeProvider;
        }

        [HttpGet]
        [Route(Contract.Routes.USER_INFO)]
        public IHttpActionResult GetUserInfo()
        {
            var token = this.GetToken();

            if(string.IsNullOrWhiteSpace(token))
            {
                throw new ApplicationException("Empty token in cookies");
            }

            var userInfo = _usersClient.GetUserInfo(token);
            return Ok(userInfo);
        }

        [HttpPut]
        [Route(Contract.Routes.LOGOUT)]
        public IHttpActionResult Logout()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            this.DeleteToken(response.Headers, _utcTimeProvider);
            return ResponseMessage(response);
        }
    }
}
