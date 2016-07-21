using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using PVDevelop.UCoach.Server.HttpGateway.Contract;
using PVDevelop.UCoach.Server.RestClient;

namespace PVDevelop.UCoach.Server.WebPortal.Controllers
{
    public class FacebookController : ApiController
    {
        private readonly IRestClientFactory _restClientFactory;

        public FacebookController(IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }
            _restClientFactory = restClientFactory;
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_REDIRECT_URI)]
        public IHttpActionResult RedirectToAuthorization([FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var redirectDto = 
                _restClientFactory.
                CreateGet(Role.Contract.Routes.FACEBOOK_REDIRECT_URI).
                AddParameter("redirect_uri", redirectUri).
                Execute().
                CheckGetResult().
                GetJsonContent<Role.Contract.FacebookRedirectDto>();
            return Ok(redirectDto);
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_TOKEN)]
        public IHttpActionResult GetUserProfile(
            [FromUri] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var tokenDto =
               _restClientFactory.
               CreateGet(Role.Contract.Routes.FACEBOOK_TOKEN).
               AddParameter("code", code).
               AddParameter("redirect_uri", redirectUri).
               Execute().
               CheckGetResult().
               GetJsonContent<Role.Contract.TokenDto>();

            // заполняем cookie
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var cookie = new CookieHeaderValue("access_token", tokenDto.Token);
            cookie.Expires = tokenDto.Expiration;
            cookie.Domain = Request.RequestUri.Host;
            response.Headers.AddCookies(new[] { cookie });

            return ResponseMessage(response);
        }
    }
}
