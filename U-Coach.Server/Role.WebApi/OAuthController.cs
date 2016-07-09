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
        [Route(Routes.GET_FACEBOOK_PAGE)]
        public OAuthRedirectDto RedirectToFacebook()
        {
            return _userService.RedirectToFacebookPage();
        }
    }
}
