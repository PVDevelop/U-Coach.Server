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

        public FacebookRedirectDto GetAuthorizationUrl()
        {
            return
                _restClientFactory.
                CreateGet(Routes.FACEBOOK_REDIRECT_URI).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookRedirectDto>();
        }

        public FacebookProfileDto GetProfile(FacebookCodeDto codeDto)
        {
            return
                _restClientFactory.
                CreateGet(Routes.FACEBOOK_USER_PROFILE).
                AddParameter("code", codeDto.Code).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookProfileDto>();
        }
    }
}
