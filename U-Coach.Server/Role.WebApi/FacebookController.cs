using System;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
    public class FacebookController : ApiController
    {
        #region dtos

        private class FacebookProfileDto
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        private class FacebookTokenDto
        {
            [JsonProperty(PropertyName = "access_token")]
            public string Token { get; set; }

            [JsonProperty(PropertyName = "expires_in")]
            public int ExpiredInSeconds { get; set; }

            [JsonProperty(PropertyName = "token_type")]
            public string Type { get; set; }
        }

        #endregion

        private readonly IUserService _userService;
        private readonly ISettingsProvider<IFacebookOAuthSettings> _settingsProvider;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public FacebookController(
            IUserService userService,
            ISettingsProvider<IFacebookOAuthSettings> settingsProvider,
            IUtcTimeProvider utcTimeProvider)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _userService = userService;
            _settingsProvider = settingsProvider;
            _utcTimeProvider = utcTimeProvider;
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_REDIRECT_URI)]
        public IHttpActionResult RedirectToAuthorization(
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var settings = _settingsProvider.Settings;

            var uriBuilder = new UriBuilder("https://www.facebook.com/dialog/oauth");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_id"] = settings.ClientId;
            parameters["redirect_uri"] = redirectUri;
            parameters["scope"] = "public_profile";
            uriBuilder.Query = parameters.ToString();

            var resultDto = new FacebookRedirectDto()
            {
                Uri = uriBuilder.ToString()
            };

            return base.Ok(resultDto);
        }

        [HttpGet]
        [Route(Routes.FACEBOOK_TOKEN)]
        public IHttpActionResult GetToken(
            [FromUri(Name = "code")] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var facebookTokenDto = GetFacebookToken(code, redirectUri);
            var profileDto = GetFacebookProfile(facebookTokenDto.Token);

            var expiration = _utcTimeProvider.UtcNow.AddSeconds(facebookTokenDto.ExpiredInSeconds);

            var userId = new UserId(AuthSystems.FACEBOOK_SYSTEM_NAME, profileDto.Id);
            var authSystemToken = new AuthSystemToken(facebookTokenDto.Token, expiration);
            var token = _userService.RegisterUserToken(userId, authSystemToken);

            var tokenDto = new TokenDto(token.Id.Token, token.Expiration);

            return Ok(tokenDto);
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
