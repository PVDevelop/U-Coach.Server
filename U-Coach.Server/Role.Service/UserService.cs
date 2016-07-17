using System;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(
            ITokenGenerator tokenGenerator,
            IUserRepository userRepository,
            ITokenRepository tokenRepository)
        {
            if(tokenGenerator == null)
            {
                throw new ArgumentNullException(nameof(tokenGenerator));
            }
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            if(tokenRepository == null)
            {
                throw new ArgumentNullException(nameof(tokenRepository));
            }

            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public TokenId RegisterUserToken(AuthTokenParams authTokenParams)
        {
            if(authTokenParams == null)
            {
                throw new ArgumentNullException(nameof(authTokenParams));
            }

            User user;
            var userId = new UserId(authTokenParams.AuthSystemName, authTokenParams.AuthUserId);
            if (!_userRepository.TryGet(userId, out user))
            {
                user = new User(userId);
                _userRepository.Insert(user);
            }

            var generatedToken = _tokenGenerator.Generate(user, authTokenParams);
            var tokenId = new TokenId(generatedToken);

            Token token;
            if (!_tokenRepository.TryGet(tokenId, out token))
            {
                token = new Token(tokenId, userId, authTokenParams);
                _tokenRepository.Insert(token);
            }

            return token.Id;
        }
    }
}
