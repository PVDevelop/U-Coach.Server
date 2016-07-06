using System;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.RestClient;

namespace PVDevelop.UCoach.Server.Auth.RestClient
{
    public class RestUsersClient : IUsersClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public RestUsersClient(IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException("restClientFactory");
            }
            _restClientFactory = restClientFactory;
        }

        public string Create(CreateUserDto userDto)
        {
            return 
                _restClientFactory.
                CreatePost(Routes.CREATE_USER).
                AddBody(userDto).
                Execute().
                Content;
        }
    }
}
