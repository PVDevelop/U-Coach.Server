using System;
using PVDevelop.UCoach.Server.Role.Domain.Validator;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class UserService : IUserService
    {
        public static TimeSpan TokenDuration
        {
            get { return TimeSpan.FromDays(10); }
        }

        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUtcTimeProvider _timeProvider;
        private readonly IAuthTokenValidatorContainer _validatorContainer;

        public UserService(
            ITokenGenerator tokenGenerator,
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IUtcTimeProvider timeProvider,
            IAuthTokenValidatorContainer validatorContainer)
        {
            if (tokenGenerator == null)
            {
                throw new ArgumentNullException(nameof(tokenGenerator));
            }
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            if (tokenRepository == null)
            {
                throw new ArgumentNullException(nameof(tokenRepository));
            }
            if (timeProvider == null)
            {
                throw new ArgumentNullException(nameof(timeProvider));
            }
            if (validatorContainer == null)
            {
                throw new ArgumentNullException(nameof(validatorContainer));
            }

            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _timeProvider = timeProvider;
            _validatorContainer = validatorContainer;
        }

        public Token RegisterUserToken(
            UserId userId,
            AuthSystemToken authToken)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            User user;
            if (!_userRepository.TryGet(userId, out user))
            {
                user = new User(userId);
                _userRepository.Insert(user);
            }

            var generatedToken = _tokenGenerator.Generate(user, authToken);
            var tokenId = new TokenId(generatedToken);

            Token privateToken;
            if (!_tokenRepository.TryGet(tokenId, out privateToken))
            {
                var expiration = _timeProvider.UtcNow + TokenDuration;
                privateToken = new Token(tokenId, userId, authToken, expiration);
                _tokenRepository.Insert(privateToken);
            }

            return privateToken;
        }

        public User GetUserByToken(TokenId tokenId)
        {
            if (tokenId == null)
            {
                throw new ArgumentNullException(nameof(tokenId));
            }

            Token token;
            if (!_tokenRepository.TryGet(tokenId, out token))
            {
                throw new NotAuthorizedException(string.Format("Token {0} not found", tokenId.Token));
            }

            ValidateToken(token);

            User user;
            if (!_userRepository.TryGet(token.UserId, out user))
            {
                // Ошибка - токен есть, а пользователя нет!
                throw new ApplicationException(string.Format("User {0} not found", tokenId.Token));
            }

            return user;
        }

        private void ValidateToken(Token token)
        {
            var validator = _validatorContainer.GetValidator(token.UserId.AuthSystemName);
            validator.Validate(token.AuthToken);
        }
    }
}
