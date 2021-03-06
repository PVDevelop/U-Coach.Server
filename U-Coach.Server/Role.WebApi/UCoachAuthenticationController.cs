﻿using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;
using Routes = PVDevelop.UCoach.Server.Role.Contract.Routes;
using TokenDto = PVDevelop.UCoach.Server.Role.Contract.TokenDto;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
	public class UCoachAuthenticationController : ApiController
	{
		private readonly Auth.Contract.IUsersClient _authUsersClient;
		private readonly IUserService _userService;

		public UCoachAuthenticationController(
			Auth.Contract.IUsersClient authUsersClient,
			IUserService userService)
		{
			if (authUsersClient == null)
			{
				throw new ArgumentNullException(nameof(authUsersClient));
			}
			if (userService == null)
			{
				throw new ArgumentNullException(nameof(userService));
			}

			_authUsersClient = authUsersClient;
			_userService = userService;
		}

		[HttpGet]
		[Route(Routes.UCOACH_TOKEN)]
		public IHttpActionResult GetToken(
			[FromUri(Name = "login")] string login,
			[FromUri(Name = "password")] string password)
		{
#warning реализовать
			throw new NotImplementedException();
			//var passwordDto = new PasswordDto(password);
			//var authToken = _authUsersClient.Logon(login, passwordDto);

			//var authUserId = new AuthUserId(AuthSystems.UCOACH_SYSTEM_NAME, authToken.UserId);
			//var authSystemToken = new AuthSystemToken(authToken.Key, authToken.ExpiryDate);
			//var token = _userService.RegisterUserToken(authUserId, authSystemToken);

			//var tokenDto = new TokenDto(token.Id.Token, token.Expiration);

			//return Ok(tokenDto);
		}
	}
}
