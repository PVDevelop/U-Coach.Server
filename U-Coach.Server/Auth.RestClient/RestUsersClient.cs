﻿using System;
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

        public TokenDto Create(UserDto user)
        {
            return
                _restClientFactory.
                CreatePost(Routes.CREATE_USER).
                AddBody(user).
                Execute().
                CheckPostResult().
                GetContent<TokenDto>();
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
    }
}
