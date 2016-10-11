using System;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.RestClient
{
    public class RestFacebookClient : IFacebookClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public RestFacebookClient(IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }

            _restClientFactory = restClientFactory;
        }

        public FacebookRedirectDto GetAuthPageUri(string redirectUri)
        {
            return
                _restClientFactory.
                CreateGet(Routes.FACEBOOK_REDIRECT_URI).
                AddParameter("redirect_uri", redirectUri).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookRedirectDto>();
        }

        public TokenDto ExchangeCodeByToken(string code, string redirectUri)
        {
            return
                _restClientFactory.
                CreateGet(Routes.FACEBOOK_TOKEN).
                AddParameter("code", code).
                AddParameter("redirect_uri", redirectUri).
                Execute().
                CheckGetResult().
                GetJsonContent<TokenDto>();
        }
    }
}
