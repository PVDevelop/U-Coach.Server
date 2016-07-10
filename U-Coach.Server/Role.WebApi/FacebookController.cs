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
            if(settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            _settingsProvider = settingsProvider;
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_REDIRECT_URI)]
        public IHttpActionResult RedirectToAuthorization()
        {
            var settings = _settingsProvider.Settings;
            var redirectUri = string.Format(
               "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}",
               settings.ClientId,
               settings.UriRedirectToCode,
               "public_profile");
            return base.Redirect(redirectUri);
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_USER_PROFILE)]
        public IHttpActionResult GetUserProfile([FromUri] string code)
        {
            var tokenDto = GetFacebookToken(code);
            var profile = GetFacebookProfile(tokenDto.Token);
            return Ok(profile);
        }

        private static IRestClientFactory GetFacebookGraphClientFactory()
        {
            var connectionStringProvider = new SimpleConnectionStringProvider("https://graph.facebook.com");
            return new RestClientFactory(connectionStringProvider);
        }

        private FacebookTokenDto GetFacebookToken(string code)
        {
            var settings = _settingsProvider.Settings;
            return
                GetFacebookGraphClientFactory().
                CreateGet("v2.5/oauth/access_token").
                AddParameter("client_id", settings.ClientId).
                AddParameter("redirect_uri", settings.UriRedirectToCode).
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
