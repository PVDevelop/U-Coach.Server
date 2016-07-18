using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public sealed class TokenService :
        ITokenService
    {
        private readonly ILogger _logger = LoggerFactory.CreateLogger<TokenService>();

        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenFactory _tokenFactory;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public TokenService(
            ITokenFactory tokenFactory,
            ITokenRepository tokenRepository,
            ITokenService tokenService,
            IUtcTimeProvider utcTimeProvider)
        {
            if (tokenRepository == null)
            {
                throw new ArgumentNullException(nameof(tokenRepository));
            }
            if (tokenFactory == null)
            {
                throw new ArgumentNullException(nameof(tokenFactory));
            }
            if (tokenService == null)
            {
                throw new ArgumentNullException(nameof(tokenService));
            }
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _utcTimeProvider = utcTimeProvider;
            _tokenRepository = tokenRepository;
            _tokenFactory = tokenFactory;
            _tokenService = tokenService;
        }

        public Token ReqistrToken(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            _logger.Debug("Создание токена для пользователя {0}", userId);

            var token = _tokenFactory.CreateToken(userId);
            _tokenRepository.AddToken(token);

            _logger.Debug("Создание токена для пользователя {0} произошло успешно", userId);

            return token;
        }

        public void CloseToken(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            _logger.Debug("Удаление токена {0}", token);

            _tokenRepository.CloseToken(token);

            _logger.Debug("Удаление токена {0} произошло успешно", token);
        }

        public bool ValidateToken(string localToken)
        {
            if (String.IsNullOrEmpty(localToken))
            {
                throw new ArgumentNullException(nameof(localToken));
            }

            _logger.Debug("Валидация токена для пользователя {0}", localToken);

            Token serverToken = _tokenRepository.GetToken(localToken);

            if (serverToken == null || _utcTimeProvider.UtcNow > serverToken.ExpiryDate)
            {
                _logger.Debug("Валидация токена {0} завершена. Token невалиден", localToken);
                return false;
            }
            else
            {
                _logger.Debug("Валидация токена {0} произошла успешно", localToken);
                return true;
            }
        }
    }
}
