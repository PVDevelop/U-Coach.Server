using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public class FacebookController : ApiController
    {
        private readonly IFacebookClient _facebookClient;
        private readonly ITokenManager _tokenManager;

        public FacebookController(
            IFacebookClient facebookClient,
            ITokenManager tokenManager)
        {
            if (facebookClient == null)
            {
                throw new ArgumentNullException(nameof(facebookClient));
            }
            if (tokenManager == null)
            {
                throw new ArgumentNullException(nameof(tokenManager));
            }

            _facebookClient = facebookClient;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        [Route(Contract.Routes.FACEBOOK_REDIRECT_URI)]
        public IHttpActionResult RedirectToAuthorization(
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var redirectDto = _facebookClient.GetAuthPageUri(redirectUri);
            return base.Ok(redirectDto);
        }

        [HttpGet]
        [Route(Contract.Routes.FACEBOOK_TOKEN)]
        public IHttpActionResult GetToken(
            [FromUri(Name = "code")] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var tokenDto = _facebookClient.ExchangeCodeByToken(code, redirectUri);

            // заполняем cookie
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _tokenManager.SetToken(this, response.Headers, tokenDto);

            return ResponseMessage(response);
        }

        private static IRestClientFactory GetFacebookGraphClientFactory()
        {
            var connectionStringProvider = new SimpleConnectionStringProvider("https://graph.facebook.com");
            return new RestClientFactory(connectionStringProvider);
        }
    }
}
