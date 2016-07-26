using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVDevelop.UCoach.Server.HttpGateway.Contract;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public class UCoachAuthenticationController : ApiController
    {
        private readonly IUCoachAuthClient _ucAuthClient;
        private readonly ITokenManager _tokenManager;

        public UCoachAuthenticationController(
            IUCoachAuthClient ucAuthClient,
            ITokenManager tokenManager)
        {
            if (ucAuthClient == null)
            {
                throw new ArgumentNullException(nameof(ucAuthClient));
            }
            if(tokenManager == null)
            {
                throw new ArgumentNullException(nameof(tokenManager));
            }

            _ucAuthClient = ucAuthClient;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        [Route(Contract.Routes.UCOACH_TOKEN)]
        public IHttpActionResult GetToken(
            [FromUri(Name = "login")] string login,
            [FromUri(Name = "password")] string password)
        {
            var tokenDto = _ucAuthClient.GetToken(login, password);

            // заполняем cookie
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _tokenManager.SetToken(this, response.Headers, tokenDto);

            return ResponseMessage(response);
        }
    }
}
