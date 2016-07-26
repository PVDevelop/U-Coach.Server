using System;
using System.Web;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.FacebookContract;

namespace PVDevelop.UCoach.Server.Role.FacebookRestClient
{
    public class RestFacebookClient : IFacebookClient
    {
        private readonly ISettingsProvider<IFacebookOAuthSettings> _settingsProvider;

        public RestFacebookClient(ISettingsProvider<IFacebookOAuthSettings> settingsProvider)
        {
            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            _settingsProvider = settingsProvider;
        }

        public FacebookTokenDto GetFacebookToken(
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

        public FacebookProfileDto GetFacebookProfile(string token)
        {
            return
                GetFacebookGraphClientFactory().
                CreateGet("me").
                AddParameter("access_token", token).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookProfileDto>();
        }

        private static IRestClientFactory GetFacebookGraphClientFactory()
        {
            var connectionStringProvider = new SimpleConnectionStringProvider("https://graph.facebook.com");
            return new RestClientFactory(connectionStringProvider);
        }

        public Uri GetAuthorizationUri(string redirectUri)
        {
            var uriBuilder = new UriBuilder("https://www.facebook.com/dialog/oauth");

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_id"] = _settingsProvider.Settings.ClientId;
            parameters["redirect_uri"] = redirectUri;
            parameters["scope"] = "public_profile";
            uriBuilder.Query = parameters.ToString();

            return uriBuilder.Uri;
        }
    }
}
