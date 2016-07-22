using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.HttpGateway.Contract;
using PVDevelop.UCoach.Server.RestClient;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller
{
    public class UsersController : ApiController
    {
        private readonly IRestClientFactory _restClientFactory;

        public UsersController(IRestClientFactory restClientFactory)
        {
            if (restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }

            _restClientFactory = restClientFactory;
        }

        [HttpGet]
        [Route(Routes.USER_INFO)]
        public IHttpActionResult GetUserInfo()
        {
            var token = this.GetToken();

            if(string.IsNullOrWhiteSpace(token))
            {
                throw new ApplicationException("Empty token in cookies");
            }

            var userInfo = 
                _restClientFactory.
                CreateGet(Role.Contract.Routes.USER_INFO, token).
                Execute().
                CheckGetResult().
                GetJsonContent<Role.Contract.UserInfoDto>();

            return Ok(userInfo);
        }
    }
}
