using System;
using PVDevelop.UCoach.Server.Logging;
using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger = LoggerFactory.CreateLogger<UserService>();
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            if(userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            _userRepository = userRepository;
        }

        /// <summary>
        /// Создает нового пользователя.
        /// Если пользователь уже существует, кидает исключение DuplicateUserException.
        /// </summary>
        /// <param name="userParams">Параметры нового пользователя</param>
        public void Create(CreateUserParams userParams)
        {
            if (userParams == null)
            {
                throw new ArgumentNullException("userParams");
            }

            _logger.Debug("Создаю пользователя {0}.", userParams.Login);

            var user = new User(userParams.Login);
            user.SetPassword(userParams.Password);
            _userRepository.Insert(user);

            _logger.Info("Пользователь {0} создан.", userParams.Login);
        }

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует пользователя
        /// </summary>
        /// <param name="userParams">Параметры аутентификацити</param>
        /// <returns>Токен аутентификации</returns>
        public string Logon(LogonUserParams userParams)
        {
            if (userParams == null)
            {
                throw new ArgumentNullException("userParams");
            }

            _logger.Debug("Логиню пользователя {0}.", userParams.Login);

            var user = _userRepository.FindByLogin(userParams.Login);;
            var token = user.Logon(userParams.Password);
            _userRepository.Update(user);

            _logger.Info("Пользователь {0} залогинен.", userParams.Login);

            return token;
        }

        /// <summary>
        /// Выводит пользователя из системы
        /// </summary>
        /// <param name="userParams">Параметры пользователя</param>
        public void LogoutByPassword(LogoutByPasswordUserParams userParams)
        {
            if (userParams == null)
            {
                throw new ArgumentNullException("userParams");
            }

            _logger.Debug("Логаут пользователя {0}.", userParams.Login);

            var user = _userRepository.FindByLogin(userParams.Login);
            user.Logout(userParams.Password);
            _userRepository.Update(user);

            _logger.Info("Логаут пользователя {0} выполнен.", userParams.Login);
        }

        /// <summary>
        /// Проверяет токен пользователя
        /// </summary>
        /// <param name="tokenParams">Параметры токена</param>
        public void ValidateToken(ValidateTokenParams tokenParams)
        {
            if (tokenParams == null)
            {
                throw new ArgumentNullException("tokenParams");
            }

            _logger.Debug("Валидирую токен пользователя {0}.", tokenParams.Login);

            var user = _userRepository.FindByLogin(tokenParams.Login);
            user.ValidateToken(tokenParams.Token);

            _logger.Info("Токен пользователя {0} валиден.", tokenParams.Login);
        }
    }
}
