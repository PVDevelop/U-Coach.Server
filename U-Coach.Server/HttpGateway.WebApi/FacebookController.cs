using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.HttpGateway.Contract;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
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

        public const string FACEBOOK_SYSTEM_NAME = "Facebook";

        private readonly IRestClientFactory _restClientFactory;
        private readonly ISettingsProvider<IFacebookOAuthSettings> _settingsProvider;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public FacebookController(
            IRestClientFactory restClientFactory,
            ISettingsProvider<IFacebookOAuthSettings> settingsProvider,
            IUtcTimeProvider utcTimeProvider)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }
            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _restClientFactory = restClientFactory;
            _settingsProvider = settingsProvider;
            _utcTimeProvider = utcTimeProvider;
        }

        [HttpGet]
        [Route(Contract.Routes.FACEBOOK_REDIRECT_URI)]
        public IHttpActionResult RedirectToAuthorization(
            [FromUri(Name = "redirect_uri")]string redirectUri)
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
        [Route(Contract.Routes.FACEBOOK_TOKEN)]
        public IHttpActionResult GetUserProfile(
            [FromUri] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var facebookTokenDto = GetFacebookToken(code, redirectUri);
            var profileDto = GetFacebookProfile(facebookTokenDto.Token);

            var expiration = _utcTimeProvider.UtcNow.AddSeconds(facebookTokenDto.ExpiredInSeconds);
            var authRegisterDto = new AuthUserRegisterDto(facebookTokenDto.Token, expiration);

            var tokenDto = 
                _restClientFactory.
                CreatePut(Role.Contract.Routes.REGISTER_USER, FACEBOOK_SYSTEM_NAME, profileDto.Id).
                AddBody(authRegisterDto).
                Execute().
                CheckPutResult().
                GetJsonContent<TokenDto>();

            // заполняем cookie
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            this.SetToken(response.Headers, tokenDto);

            return ResponseMessage(response);
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
