using System;
using PVDevelop.UCoach.Server.Role.Domain.AuthTokenValidation;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly IAuthTokenValidatorContainer _validatorContainer;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public TokenValidationService(
            IAuthTokenValidatorContainer validatorContainer,
            IUtcTimeProvider utcTimeProvider)
        {
            if (validatorContainer == null)
            {
                throw new ArgumentNullException(nameof(validatorContainer));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _validatorContainer = validatorContainer;
            _utcTimeProvider = utcTimeProvider;
        }

        public void Validate(Token token, string authSystemName)
        {
            token.Validate(_utcTimeProvider.UtcNow);

            var validator = _validatorContainer.GetValidator(authSystemName);
            validator.Validate(token.AuthToken);
        }
    }
}
