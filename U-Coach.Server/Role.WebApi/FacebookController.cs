using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public const string FACEBOOK_SYSTEM_NAME = "Facebook";

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

        private readonly ISettingsProvider<IFacebookOAuthSettings> _settingsProvider;
        private readonly IUserService _userService;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public FacebookController(
            ISettingsProvider<IFacebookOAuthSettings> settingsProvider,
            IUserService userService,
            IUtcTimeProvider utcTimeProvider)
        {
            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _settingsProvider = settingsProvider;
            _userService = userService;
            _utcTimeProvider = utcTimeProvider;
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
            CookieHeaderValue requestCookie = Request.Headers.GetCookies("access_token").FirstOrDefault();
            if (requestCookie != null)
            {
                var token = requestCookie["access_token"].Value;
            }

            var tokenDto = GetFacebookToken(code, redirectUri);
            var profileDto = GetFacebookProfile(tokenDto.Token);

            var authTokenParams = new AuthTokenParams(FACEBOOK_SYSTEM_NAME, profileDto.Id, tokenDto.Token);
            var tokenId = _userService.RegisterUserToken(authTokenParams);

            var tokenExpiration = _utcTimeProvider.UtcNow + TimeSpan.FromSeconds(tokenDto.ExpiredInSeconds);
            var connectionDto = new FacebookConnectionDto()
            {
                Id = profileDto.Id,
                Name = profileDto.Name,
                Token = tokenDto.Token,
                TokenExpiration = tokenExpiration
            };

            var response = Request.CreateResponse(HttpStatusCode.OK, connectionDto);

            var cookie = new CookieHeaderValue("access_token", tokenId.Token);
            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";
            response.Headers.AddCookies(new[] { cookie });

            return ResponseMessage(response);
            //return Ok(connectionDto);
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
