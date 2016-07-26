using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
    public class FacebookController : ApiController
    {
        private readonly IUserService _userService;
        private readonly FacebookContract.IFacebookClient _facebookClient;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public FacebookController(
            IUserService userService,
            FacebookContract.IFacebookClient facebookClient,
            IUtcTimeProvider utcTimeProvider)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (facebookClient == null)
            {
                throw new ArgumentNullException(nameof(facebookClient));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _userService = userService;
            _facebookClient = facebookClient;
            _utcTimeProvider = utcTimeProvider;
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_REDIRECT_URI)]
        public IHttpActionResult RedirectToAuthorization(
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var uri = _facebookClient.GetAuthorizationUri(redirectUri);
            var resultDto = new FacebookRedirectDto()
            {
                Uri = uri.ToString()
            };

            return base.Ok(resultDto);
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_TOKEN)]
        public IHttpActionResult GetToken(
            [FromUri(Name = "code")] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var facebookTokenDto = _facebookClient.GetFacebookToken(code, redirectUri);
            var profileDto = _facebookClient.GetFacebookProfile(facebookTokenDto.Token);

            var expiration = _utcTimeProvider.UtcNow.AddSeconds(facebookTokenDto.ExpiredInSeconds);

            var userId = new UserId(AuthSystems.FACEBOOK_SYSTEM_NAME, profileDto.Id);
            var authSystemToken = new AuthSystemToken(facebookTokenDto.Token, expiration);
            var token = _userService.RegisterUserToken(userId, authSystemToken);

            var tokenDto = new TokenDto(token.Id.Token, token.Expiration);

            return Ok(tokenDto);
        }
    }
}
