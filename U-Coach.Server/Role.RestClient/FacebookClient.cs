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

        public string GetAuthorizationUrl()
        {
            return
                _restClientFactory.
                CreateGet(Routes.FACEBOOK_REDIRECT_URI).
                Execute().
                GetContent();
        }
    }
}
