﻿using System;
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
            var uri = RestHelper.FormatUri(Routes.LOGON_USER, logonUserDto.Login);
            return
                _restClientFactory.
                CreatePost(uri).
                AddBody(logonUserDto.Password).
                Execute().
                GetContentOrThrow();
        }

        public void LogoutByPassword(LogoutByPasswordUserDto logoutByPasswordUserDto)
        {
            var uri = RestHelper.FormatUri(Routes.LOGOUT_USER, logoutByPasswordUserDto.Login);
            _restClientFactory.
                CreatePost(uri).
                AddBody(logoutByPasswordUserDto.Password).
                Execute().
                CheckResult();
        }

        public void ValidateToken(ValidateTokenDto tokenDto)
        {
            var uri = RestHelper.FormatUri(Routes.VALIDATE_USER_TOKEN, tokenDto.Login);
            _restClientFactory.
                CreatePost(uri).
                AddBody(tokenDto.Token).
                Execute().
                CheckResult();
        }
    }
}
