using System;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger = LoggerFactory.CreateLogger<UserService>();

        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IConfirmationService _confirmationService;
        private readonly ITokenService _tokenService;

        public UserService(
            IUserRepository userRepository,
            IUserFactory userFactory,
            IConfirmationService confirmationService,
            ITokenService tokenService)
        {
            if(userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            if (userFactory == null)
            {
                throw new ArgumentNullException(nameof(userFactory));
            }
            if (confirmationService == null)
            {
                throw new ArgumentNullException(nameof(confirmationService));
            }
            if (tokenService == null)
            {
                throw new ArgumentNullException(nameof(tokenService));
            }

            _userRepository = userRepository;
            _userFactory = userFactory;
            _confirmationService = confirmationService;
            _tokenService = tokenService;
        }

        public CreateUserResultDto Create(CreateUserDto userParams)
        {
            if (userParams == null)
            {
                throw new ArgumentNullException(nameof(userParams));
            }

            _logger.Debug("Создаю пользователя {0}.", userParams.Login);

            var user = _userFactory.CreateUser(userParams.Login, userParams.Password);
            _userRepository.Insert(user);

            _confirmationService.CreateConfirmation(user.Id);

            _logger.Info("Пользователь {0} создан.", userParams.Login);

            return new CreateUserResultDto()
            {
                Id = user.Id.ToString()
            };
        }

        public LogonUserResultDto Logon(LogonUserDto userParams)
        {
            if (userParams == null)
            {
                throw new ArgumentNullException(nameof(userParams));
            }

            _logger.Debug("Логиню пользователя {0}.", userParams.Login);

            var user = _userRepository.FindByLogin(userParams.Login);
            user.CheckPassword(userParams.Password);
            var token = _tokenService.ReqistrToken(user.Id);

            _userRepository.Update(user);

            _logger.Info("Пользователь {0} залогинен.", userParams.Login);

            return new LogonUserResultDto()
            {
                Token = token.Key
            };
        }

        public void Logout(ValidateTokenDto tokenParams)
        {
            if (tokenParams == null)
            {
                throw new ArgumentNullException(nameof(tokenParams));
            }

            _logger.Debug("Выход пользователя {0} с токеном {1}.", tokenParams.Login);

            var user = _userRepository.FindByLogin(tokenParams.Login);

            _tokenService.CloseToken(tokenParams.Token);

            _logger.Info("Пользователь {0} вышел из системы", tokenParams.Login);
        }

        public void ValidateToken(ValidateTokenDto tokenParams)
        {
            if (tokenParams == null)
            {
                throw new ArgumentNullException(nameof(tokenParams));
            }

            _logger.Debug("Валидирую токен пользователя {0}.", tokenParams.Login);

            _tokenService.ValidateToken(tokenParams.Token);

            _logger.Info("Токен пользователя {0} валиден.", tokenParams.Login);
        }

        public void Confirm(string login, string key)
        {
            if (String.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(login));
            }

            _logger.Debug("Подтверждение пользователя {0}.", login);

            var user = _userRepository.FindByLogin(login);

            if (user.IsConfirmation)
            {
                _logger.Info("Подтверждение пользователя {0} не требуется", login);
            }
            else
            {
                user.IsConfirmation = _confirmationService.Confirm(user.Id, key);
                _userRepository.Update(user);
            }

            _logger.Info("Подтверждение пользователя {0} завершено.", login);

        }
    }
}
