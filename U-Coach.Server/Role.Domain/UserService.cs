using System;
using PVDevelop.UCoach.Server.Role.Domain.AuthTokenValidation;
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
        private readonly ITokenValidationService _tokenValidator;

        public UserService(
            ITokenGenerator tokenGenerator,
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            ITokenValidationService tokenValidator,
            IUtcTimeProvider timeProvider)
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
            if(tokenValidator == null)
            {
                throw new ArgumentNullException(nameof(tokenValidator));
            }
            if (timeProvider == null)
            {
                throw new ArgumentNullException(nameof(timeProvider));
            }

            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _tokenValidator = tokenValidator;
            _timeProvider = timeProvider;
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

            _tokenValidator.Validate(token);

            User user;
            if (!_userRepository.TryGet(token.UserId, out user))
            {
                // Ошибка - токен есть, а пользователя нет!
                throw new ApplicationException(string.Format("User {0} not found", tokenId.Token));
            }

            return user;
        }

        public void DeleteToken(TokenId tokenId)
        {
            if(tokenId == null)
            {
                throw new ArgumentNullException(nameof(tokenId));
            }

            Token token;
            if (!_tokenRepository.TryGet(tokenId, out token))
            {
                return;
            }

            token.Delete();
            _tokenRepository.Update(token);
        }
    }
}
