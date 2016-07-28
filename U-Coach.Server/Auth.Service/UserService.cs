using System;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;
using Utilities;

#warning Давай этот проект сольем в Auth.Domain
#warning возможно нужно учитывать статус пользователя
namespace PVDevelop.UCoach.Server.Auth.Service
{

    public class UserService : 
        IUserService
    {
        private readonly ILogger _logger = LoggerFactory.CreateLogger<UserService>();

        private readonly IUserValidator _userValidator;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfirmationRepository _confirmationRepository;

        private readonly IConfirmationProducer _confirmationProducer;
        private readonly IKeyGeneratorService _keyGeneratorService;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public UserService(
            IUserValidator userValidator,
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IConfirmationRepository confirmationRepository,
            IConfirmationProducer confirmationProducer,
            IKeyGeneratorService keyGeneratorService,
            IUtcTimeProvider utcTimeProvider)
        {
            userValidator.NullValidate(nameof(userValidator));
            userRepository.NullValidate(nameof(userRepository));
            tokenRepository.NullValidate(nameof(tokenRepository));
            confirmationRepository.NullValidate(nameof(confirmationRepository));
            confirmationProducer.NullValidate(nameof(confirmationProducer));
            keyGeneratorService.NullValidate(nameof(keyGeneratorService));
            utcTimeProvider.NullValidate(nameof(utcTimeProvider));

            _userValidator = userValidator;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _confirmationRepository = confirmationRepository;
            _confirmationProducer = confirmationProducer;
            _keyGeneratorService = keyGeneratorService;
            _utcTimeProvider = utcTimeProvider;
        }

        public Token CreateUser(string login, string password)
        {
            try
            {
                _userValidator.ValidateLogin(login);
                _userValidator.ValidatePassword(password);

                _logger.Debug("Создаю пользователя '{0}'.", login);

                var user = new User(_keyGeneratorService.GenerateUserId())
                {
                    Login = login,
                    CreationTime = _utcTimeProvider.UtcNow,
                    Status = UserStatus.Unconfirmed
                };
                user.SetPassword(password);
                _userRepository.Insert(user);

                _logger.Debug("Создаю ключ подтверждения для пользователя '{0}'.", login);
                var confirmation = new Confirmation(
                    userId: user.Id, 
                    key: _keyGeneratorService.GenerateUserId(),
                    creationTime: _utcTimeProvider.UtcNow);
                _confirmationRepository.Replace(confirmation);

                _logger.Debug("Отправление ключ пользователю");
                _confirmationProducer.Produce(login, confirmation.Key);

                _logger.Debug("Создаю токен доступа для пользователя '{0}'.", login);
                var token = new Token(
                    userId: user.Id,
                    key: _keyGeneratorService.GenerateTokenKey(),
                    utcTimeProvider: _utcTimeProvider);
                _tokenRepository.AddToken(token);

                _logger.Info("Пользователь {0} создан.", login);
                return token;
            }
            catch
            {
                _logger.Info("Пользователь {0} не создан.", login);
                throw;
            }
        }

        public Token Logon(string login, string password)
        {
            _logger.Debug("Логиню пользователя {0}.", login);

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException(nameof(login));
            }

            var user = _userRepository.FindByLogin(login);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            user.CheckPassword(password);

            _logger.Debug("Создаю токен доступа для пользователя '{0}'.", login);
            var token = new Token(
                    userId: user.Id,
                    key: _keyGeneratorService.GenerateTokenKey(),
                    utcTimeProvider: _utcTimeProvider);
            _tokenRepository.AddToken(token);

            _logger.Info("Пользователь {0} залогинен.", login);

            return token;
        }

        public void ValidateToken(string token)
        {
            _logger.Debug("Валидирую токен пользователя");

            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentException(nameof(token));
            }

            Token serverToken = _tokenRepository.GetToken(token);
            if (serverToken == null || _utcTimeProvider.UtcNow > serverToken.ExpiryDate)
            {
                throw new InvalidTokenException();
            }

            _logger.Info("Токен валиден.");
        }

        public void Confirm(string key)
        {
            _logger.Debug("Подтверждение пользователя");
            key.NullOrEmptyValidate(nameof(key));

            var confiramtion = _confirmationRepository.FindByConfirmation(key);
            if (confiramtion == null)
            {
                throw new ConfirmationNotFoundException();
            }

            var user = _userRepository.FindById(confiramtion.UserId);

            if (user.Status != UserStatus.Confirmed)
            {
                user.Status = UserStatus.Confirmed;
                _userRepository.Update(user);
            }

            _logger.Info("Подтверждение пользователя завершено.");
        }
    }
}
