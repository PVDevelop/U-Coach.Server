using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Service;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
    public class FacebookController : ApiController
    {
        private readonly ISettingsProvider<IFacebookOAuthSettings> _settingsProvider;

        public FacebookController(
            ISettingsProvider<IFacebookOAuthSettings> settingsProvider)
        {
            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            _settingsProvider = settingsProvider;
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_REDIRECT_URI)]
        public IHttpActionResult RedirectToAuthorization([FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var settings = _settingsProvider.Settings;

#warning переделать на uri_builder
            var resultDto = new FacebookRedirectDto()
            {
                Uri = string.Format(
                "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}",
                settings.ClientId,
                redirectUri,
                "public_profile")
            };
            return base.Ok(resultDto);
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_USER_PROFILE)]
        public IHttpActionResult GetUserProfile(
            [FromUri] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var tokenDto = GetFacebookToken(code, redirectUri);
            var profile = GetFacebookProfile(tokenDto.Token);
            return Ok(profile);
        }

        private static IRestClientFactory GetFacebookGraphClientFactory()
        {
            var connectionStringProvider = new SimpleConnectionStringProvider("https://graph.facebook.com");
            return new RestClientFactory(connectionStringProvider);
        }

        private FacebookTokenDto GetFacebookToken(
            string code, 
            string redirectUri)
        {
            var settings = _settingsProvider.Settings;
            return
                GetFacebookGraphClientFactory().
                CreateGet("v2.5/oauth/access_token").
                AddParameter("client_id", settings.ClientId).
                AddParameter("redirect_uri", redirectUri).
                AddParameter("client_secret", settings.ClientSecret).
                AddParameter("code", code).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookTokenDto>();
        }

        private FacebookProfileDto GetFacebookProfile(string token)
        {
            return
                GetFacebookGraphClientFactory().
                CreateGet("me").
                AddParameter("access_token", token).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookProfileDto>();
        }
    }
}
