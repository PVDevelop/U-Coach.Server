using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.WebApi;

namespace PVDevelop.UCoach.Server.WebPortal.Controllers
{
    public class FacebookController : ApiController
    {
        private readonly IActionResultBuilderFactory _actionResultBuilderFactory;

        public FacebookController(
            IActionResultBuilderFactory actionResultBuilderFactory)
        {
            if(actionResultBuilderFactory == null)
            {
                throw new ArgumentNullException(nameof(actionResultBuilderFactory));
            }
            _actionResultBuilderFactory = actionResultBuilderFactory;
        }

        [HttpGet]
        [Route("api/facebook/authorization")]
        public async Task<IHttpActionResult> RedirectToAuthorization([FromUri(Name = "redirect_uri")]string redirectUri)
        {
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                return ResponseMessage(
                    (await builder.
                    AddParameter("redirect_uri", redirectUri).
                    BuildAsync(Routes.FACEBOOK_REDIRECT_URI)).
                    EnsureSuccessStatusCode());
            }
        }

        [HttpGet]
        [Route("api/facebook/connection")]
        public async Task<IHttpActionResult> GetUserProfile(
            [FromUri] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            using (var builder = _actionResultBuilderFactory.CreateActionResultBuilder())
            {
                return ResponseMessage(
                    (await builder.
                    AddParameter("code", code).
                    AddParameter("redirect_uri", redirectUri).
                    BuildAsync(Routes.FACEBOOK_USER_PROFILE)).
                    EnsureSuccessStatusCode());
            }
        }
    }
}
