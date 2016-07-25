﻿using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.HttpGateway.Contract;
using PVDevelop.UCoach.Server.HttpGateway.WebApi.Settings;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller
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

        private readonly IUsersClient _roleUsersClient;
        private readonly ISettingsProvider<IFacebookOAuthSettings> _settingsProvider;
        private readonly IUtcTimeProvider _utcTimeProvider;
        private readonly ITokenManager _tokenManager;

        public FacebookController(
            IUsersClient roleUsersClient,
            ISettingsProvider<IFacebookOAuthSettings> settingsProvider,
            IUtcTimeProvider utcTimeProvider,
            ITokenManager tokenManager)
        {
            if (roleUsersClient == null)
            {
                throw new ArgumentNullException(nameof(roleUsersClient));
            }
            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }
            if (tokenManager == null)
            {
                throw new ArgumentNullException(nameof(tokenManager));
            }

            _roleUsersClient = roleUsersClient;
            _settingsProvider = settingsProvider;
            _utcTimeProvider = utcTimeProvider;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        [Route(Contract.Routes.FACEBOOK_REDIRECT_URI)]
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
        [Route(Contract.Routes.FACEBOOK_TOKEN)]
        public IHttpActionResult GetToken(
            [FromUri] string code,
            [FromUri(Name = "redirect_uri")]string redirectUri)
        {
            var facebookTokenDto = GetFacebookToken(code, redirectUri);
            var profileDto = GetFacebookProfile(facebookTokenDto.Token);

            var expiration = _utcTimeProvider.UtcNow.AddSeconds(facebookTokenDto.ExpiredInSeconds);
            var userRegisterDto = new AuthUserRegisterDto(facebookTokenDto.Token, expiration);

            var tokenDto =
                _roleUsersClient.
                RegisterUser(AuthSystems.FACEBOOK_SYSTEM_NAME, profileDto.Id, userRegisterDto);

            // заполняем cookie
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            _tokenManager.SetToken(this, response.Headers, tokenDto);

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