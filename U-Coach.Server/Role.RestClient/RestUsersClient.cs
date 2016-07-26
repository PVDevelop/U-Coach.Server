using System;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.RestClient
{
    public class RestUsersClient : IUsersClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public RestUsersClient(
            IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }

            _restClientFactory = restClientFactory;
        }

        public UserInfoDto GetUserInfo(string token)
        {
            return
                _restClientFactory.
                CreateGet(Routes.USER_INFO).
                AddParameter("token", token).
                Execute().
                CheckGetResult().
                GetJsonContent<UserInfoDto>();
        }
    }
}
