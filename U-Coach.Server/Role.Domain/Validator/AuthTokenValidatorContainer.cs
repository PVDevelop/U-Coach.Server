using System;
using System.Collections.Generic;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.Domain.Validator
{
    public class AuthTokenValidatorContainer : IAuthTokenValidatorContainer
    {
        private readonly Dictionary<string, IAuthTokenValidator> _validators;

        public AuthTokenValidatorContainer(
            IAuthTokenValidator facebookValidator,
            IAuthTokenValidator uCoachValidator)
        {
            if(facebookValidator == null)
            {
                throw new ArgumentNullException(nameof(facebookValidator));
            }
            if (uCoachValidator == null)
            {
                throw new ArgumentNullException(nameof(uCoachValidator));
            }

            _validators = new Dictionary<string, IAuthTokenValidator>()
            {
                { AuthSystems.FACEBOOK_SYSTEM_NAME, facebookValidator },
                { AuthSystems.UCOACH_SYSTEM_NAME, uCoachValidator }
            };
        }

        public IAuthTokenValidator GetValidator(string authSystemName)
        {
            return _validators[authSystemName];
        }
    }
}
