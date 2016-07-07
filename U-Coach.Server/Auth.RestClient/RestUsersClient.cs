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

        public CreateUserResultDto Create(CreateUserDto createUserDto)
        {
            return
                _restClientFactory.
                CreatePost(Routes.CREATE_USER).
                AddBody(createUserDto).
                Execute().
                CheckPostResult().
                GetContent<CreateUserResultDto>();
        }

        public LogonUserResultDto Logon(LogonUserDto logonUserDto)
        {
            return
                _restClientFactory.
                CreatePut(Routes.LOGON_USER, logonUserDto.Login).
                AddBody(logonUserDto.Password).
                Execute().
                CheckPutResult().
                GetContent<LogonUserResultDto>();
        }

        public void ValidateToken(ValidateTokenDto tokenDto)
        {
            _restClientFactory.
                CreatePut(Routes.VALIDATE_USER_TOKEN, tokenDto.Login).
                AddBody(tokenDto.Token).
                Execute().
                CheckPutResult();
        }
    }
}
