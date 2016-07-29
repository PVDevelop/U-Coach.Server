using System;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.WebApi;

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

        public void Create(UserDto user)
        {
            _restClientFactory.
                CreatePost(Routes.CREATE_USER).
                AddBody(user).
                Execute().
                CheckPutResult();
        }

        public TokenDto Logon(string login, PasswordDto password)
        {
            return
                _restClientFactory.
                CreatePut(Routes.LOGON_USER, login).
                AddBody(password).
                Execute().
                CheckPutResult().
                GetContent<TokenDto>();
        }

        public void ValidateToken(TokenDto token)
        {
            _restClientFactory.
                CreatePut(Routes.VALIDATE_USER_TOKEN).
                AddBody(token).
                Execute().
                CheckPutResult();
        }

        public void Confirm(ConfirmationDto confirmation)
        {
            _restClientFactory.
                CreatePut(Routes.CONFIRM_USER).
                AddBody(confirmation).
                Execute().
                CheckPutResult();
        }

        public void ResendConfirmation(string login)
        {
            _restClientFactory.
                CreatePut(Routes.RESEND_CONFIRM).
                Execute().
                CheckPutResult();
        }
    }
}
