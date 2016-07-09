using System;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.RestClient
{
    public class RestUsersClient : IUsersClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public RestUsersClient(IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }

            _restClientFactory = restClientFactory;
        }

        public OAuthRedirectDto RedirectToFacebookPage()
        {
            return
                _restClientFactory.
                CreateGet(Routes.GET_FACEBOOK_PAGE).
                Execute().
                CheckGetResult().
                GetJsonContent<OAuthRedirectDto>();
        }
    }
}
