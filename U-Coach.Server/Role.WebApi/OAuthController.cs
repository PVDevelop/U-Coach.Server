using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Service;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
    public class OAuthController : ApiController
    {
        private readonly IUserService _userService;

        public OAuthController(IUserService userService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            _userService = userService;
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_REDIRECT)]
        public IHttpActionResult RedirectToFacebook()
        {
            var content = _userService.RedirectToFacebookPage();
            return base.Redirect(content.RedirectUri);
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_CODE)]
        public IHttpActionResult ProcessFacebookCode([FromUri] string code)
        {
            _userService.ApplyFacebookCode(new FacebookCodeDto() { Code = code });
            return null;
        }
    }
}
