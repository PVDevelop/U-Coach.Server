using System;
using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.Role.Domain.AuthTokenValidation
{
	public class UCoachValidatorAdapter : IAuthTokenValidator
	{
		private readonly IUsersClient _usersClient;

		public UCoachValidatorAdapter(IUsersClient usersClient)
		{
			if (usersClient == null)
			{
				throw new ArgumentNullException(nameof(usersClient));
			}

			_usersClient = usersClient;
		}

		public void Validate(AuthSystemToken authSystemToken)
		{
			var tokenDto = new TokenDto(authSystemToken.Token);
			_usersClient.ValidateToken(tokenDto);
		}
	}
}
