using System;
using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.Role.Domain.Validator
{
    public class UCoachValidatorAdapter : IAuthTokenValidator
    {
        private readonly IUsersClient _usersClient;

        public UCoachValidatorAdapter(IUsersClient usersClient)
        {
            if(usersClient == null)
            {
                throw new ArgumentNullException(nameof(usersClient));
            }

            _usersClient = usersClient;
        }

        public void Validate(AuthSystemToken authSystemToken)
        {
            _usersClient.ValidateToken(authSystemToken.Token);
        }
    }
}
