using System;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.RestClient
{
    public class FacebookClient : IFacebookClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public FacebookClient(IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }
            _restClientFactory = restClientFactory;
        }

        public FacebookRedirectDto GetAuthorizationUrl(string redirectUri)
        {
            return
                _restClientFactory.
                CreateGet(Routes.FACEBOOK_REDIRECT_URI).
                AddParameter("redirect_uri", redirectUri).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookRedirectDto>();
        }

        public FacebookProfileDto GetProfile(string code, string redirectUri)
        {
            return
                _restClientFactory.
                CreateGet(Routes.FACEBOOK_USER_PROFILE).
                AddParameter("code", code).
                AddParameter("redirect_uri", redirectUri).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookProfileDto>();
        }
    }
}
