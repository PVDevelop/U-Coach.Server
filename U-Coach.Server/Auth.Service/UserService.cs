using System;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;
using Utilities;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public class UserService : 
        IUserService
    {
        private readonly ILogger _logger = LoggerFactory.CreateLogger<UserService>();

        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly ITokenFactory _tokenFactory;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfirmationFactory _confirmationFactory;
        private readonly IConfirmationRepository _confirmationRepository;
        private readonly IConfirmationProducer _confirmationProducer;
        private readonly IKeyGeneratorService _keyGeneratorService;
        private readonly IUtcTimeProvider _utcTimeProvider;

        public UserService(
            IUserFactory userFactory,
            IUserRepository userRepository,
            ITokenFactory tokenFactory,
            ITokenRepository tokenRepository,
            IConfirmationFactory confirmationFactory,
            IConfirmationRepository confirmationRepository,
            IConfirmationProducer confirmationProducer,
            IKeyGeneratorService keyGeneratorService,
            IUtcTimeProvider utcTimeProvider)
        {
            userFactory.NullValidate(nameof(userFactory));
            userRepository.NullValidate(nameof(userRepository));
            tokenFactory.NullValidate(nameof(tokenFactory));
            tokenRepository.NullValidate(nameof(tokenRepository));
            confirmationFactory.NullValidate(nameof(confirmationFactory));
            confirmationRepository.NullValidate(nameof(confirmationRepository));
            confirmationProducer.NullValidate(nameof(confirmationProducer));
            keyGeneratorService.NullValidate(nameof(keyGeneratorService));
            utcTimeProvider.NullValidate(nameof(utcTimeProvider));
            
            _userFactory = userFactory;
            _userRepository = userRepository;
            _tokenFactory = tokenFactory;
            _tokenRepository = tokenRepository;
            _confirmationFactory = confirmationFactory;
            _confirmationRepository = confirmationRepository;
            _confirmationProducer = confirmationProducer;
            _keyGeneratorService = keyGeneratorService;
            _utcTimeProvider = utcTimeProvider;
        }

        public Token Create(string login, string password)
        {
            try
            {
                _logger.Debug("Создаю пользователя '{0}'.", login);

                var user = _userFactory.CreateUser(login, password);
                _userRepository.Insert(user);

                _logger.Debug("Создаю ключ подтверждения для пользователя '{0}'.", login);
                var confirmation = _confirmationFactory.CreateConfirmation(user.Id, _keyGeneratorService.GenerateConfirmationKey());
                _confirmationRepository.Replace(confirmation);

                _logger.Debug("Отправление ключ пользователю");
                _confirmationProducer.Produce(login, confirmation.Key);

                _logger.Debug("Создаю токен доступа для пользователя '{0}'.", login);
                var token = _tokenFactory.CreateToken(user.Id, _keyGeneratorService.GenerateTokenKey());
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
            var token = _tokenFactory.CreateToken(user.Id, _keyGeneratorService.GenerateTokenKey());
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
            
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException(nameof(key));
            }

            var confiramtion = _confirmationRepository.FindByConfirmation(key);
            if (confiramtion == null)
            {
                throw new ConfirmationNotFoundException();
            }

            var user = _userRepository.FindById(confiramtion.UserId);

            if (user.Status != UserStatus.Confirm)
            {
                user.Status = UserStatus.Confirm;
                _userRepository.Update(user);
            }

            _confirmationRepository.Delete(key);

            _logger.Info("Подтверждение пользователя завершено.");
        }
    }
}
