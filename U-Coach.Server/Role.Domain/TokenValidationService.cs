using System;
using PVDevelop.UCoach.Server.Role.Domain.AuthTokenValidation;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly IAuthTokenValidatorContainer _validatorContainer;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public TokenValidationService(
            ITokenRepository tokenRepository,
            IAuthTokenValidatorContainer validatorContainer,
            IUtcTimeProvider utcTimeProvider)
        {
            if (tokenRepository == null)
            {
                throw new ArgumentNullException(nameof(tokenRepository));
            }
            if (validatorContainer == null)
            {
                throw new ArgumentNullException(nameof(validatorContainer));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _tokenRepository = tokenRepository;
            _validatorContainer = validatorContainer;
            _utcTimeProvider = utcTimeProvider;
        }

        public void Validate(TokenId tokenId)
        {
            Token token;
            if (!_tokenRepository.TryGet(tokenId, out token))
            {
                throw new NotAuthorizedException(string.Format("Token {0} not found", tokenId.Token));
            }

            if(_utcTimeProvider.UtcNow > token.Expiration)
            {
                throw new NotAuthorizedException(string.Format("Token {0} expired", tokenId.Token));
            }

            var validator = _validatorContainer.GetValidator(token.UserId.AuthSystemName);
            validator.Validate(token.AuthToken);
        }
    }
}
