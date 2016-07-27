using System;
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
            AuthUserId authUserId,
            AuthSystemToken authToken)
        {
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            User user;
            if (!_userRepository.TryGetByAuthUserId(authUserId, out user))
            {
                var userId = new UserId(Guid.NewGuid());
                user = new User(userId, authUserId);
                _userRepository.Insert(user);
            }

            var generatedToken = _tokenGenerator.Generate(user, authToken);
            var tokenId = new TokenId(generatedToken);

            Token privateToken;
            if (!_tokenRepository.TryGet(tokenId, out privateToken))
            {
                var expiration = _timeProvider.UtcNow + TokenDuration;
                privateToken = new Token(tokenId, user.Id, authToken, expiration);
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

            User user;
            if (!_userRepository.TryGet(token.UserId, out user))
            {
                // Ошибка - токен есть, а пользователя нет!
                throw new ApplicationException(string.Format("User {0} not found", tokenId.Token));
            }

            _tokenValidator.Validate(token, user.AuthUserId.AuthSystemName);

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
