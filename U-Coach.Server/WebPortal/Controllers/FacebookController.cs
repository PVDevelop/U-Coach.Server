using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.WebPortal.Controllers
{
    public class FacebookController : ApiController
    {
        private readonly IFacebookClient _facebookClient;

        public FacebookController(IFacebookClient facebookClient)
        {
            if(facebookClient == null)
            {
                throw new ArgumentNullException(nameof(facebookClient));
            }
            _facebookClient = facebookClient;
        }

        [HttpGet]
        [Route("api/facebook/authorization")]
        public IHttpActionResult RedirectToAuthorization([FromUri(Name = "redirect_uri")]string redirectUri)
        {
            return Ok(_facebookClient.GetAuthorizationUrl(redirectUri));
        }

        [HttpGet]
        [Route("api/facebook/user_profile")]
        public IHttpActionResult GetUserProfile(
            [FromUri] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            return Ok(_facebookClient.GetProfile(code, redirectUri));
        }
    }
}
