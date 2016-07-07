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
                throw new ArgumentNullException(nameof(restClientFactory));
            }
            _restClientFactory = restClientFactory;
        }

        public string Create(CreateUserDto createUserDto)
        {
            return
                _restClientFactory.
                CreatePost(Routes.CREATE_USER).
                AddBody(createUserDto).
                Execute().
                GetContentOrThrow();
        }

        public string Logon(LogonUserDto logonUserDto)
        {
            return
                _restClientFactory.
                CreatePost(Routes.LOGON_USER, logonUserDto.Login).
                AddBody(logonUserDto.Password).
                Execute().
                GetContentOrThrow();
        }

        public void ValidateToken(ValidateTokenDto tokenDto)
        {
            _restClientFactory.
                CreatePost(Routes.VALIDATE_USER_TOKEN, tokenDto.Login).
                AddBody(tokenDto.Token).
                Execute().
                CheckResult();
        }
    }
}
