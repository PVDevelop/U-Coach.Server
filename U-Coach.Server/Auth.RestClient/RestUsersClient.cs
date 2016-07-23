using System;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.RestClient
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

        public Token Create(string login, string password)
        {
            return
                _restClientFactory.
                CreatePost(Routes.CREATE_USER).
                AddBody(login).
                AddBody(password).
                Execute().
                CheckPostResult().
                GetJsonContent<CreateUserResultDto>();
        }

        public Token Logon(string login, string password)
        {
            return
                _restClientFactory.
                CreatePut(Routes.LOGON_USER, login).
                AddBody(password).
                Execute().
                CheckPutResult().
                GetJsonContent<LogonUserResultDto>();
        }

        public void ValidateToken(string token)
        {
            _restClientFactory.
                CreatePut(Routes.VALIDATE_USER_TOKEN).
                AddBody(token).
                Execute().
                CheckPutResult();
        }
    }
}
