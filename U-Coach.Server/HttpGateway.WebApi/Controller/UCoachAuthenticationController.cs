using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.HttpGateway.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller
{
    public class UCoachAuthenticationController : ApiController
    {
        private readonly Role.Contract.IUsersClient _roleUsersClient;
        private readonly Auth.Contract.IUsersClient _authUsersClient;

        public UCoachAuthenticationController(
            Auth.Contract.IUsersClient authUsersClient,
            Role.Contract.IUsersClient roleUsersClient)
        {
            if(authUsersClient == null)
            {
                throw new ArgumentNullException(nameof(authUsersClient));
            }
            if(roleUsersClient == null)
            {
                throw new ArgumentNullException(nameof(roleUsersClient));
            }

            _authUsersClient = authUsersClient;
            _roleUsersClient = roleUsersClient;
        }

        [HttpGet]
        [Route(Contract.Routes.UCOACH_TOKEN)]
        public IHttpActionResult GetToken(
            [FromUri(Name = "login")] string login,
            [FromUri(Name = "password")] string password)
        {
            var logonUserDto = new Auth.Contract.LogonUserDto()
            {
                Login = login,
                Password = password
            };

            var authToken = _authUsersClient.Logon(logonUserDto);
#warning тут надо доработать
            throw new NotImplementedException();
        }
    }
}
