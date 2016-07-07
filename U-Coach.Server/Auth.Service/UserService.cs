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

        public UserService(
            IUserRepository userRepository,
            IUserFactory userFactory)
        {
            if(userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            if (userFactory == null)
            {
                throw new ArgumentNullException(nameof(userFactory));
            }
            _userRepository = userRepository;
            _userFactory = userFactory;
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

            var user = _userRepository.FindByLogin(userParams.Login);;
            var token = user.Logon(userParams.Password);
            _userRepository.Update(user);

            _logger.Info("Пользователь {0} залогинен.", userParams.Login);

            return new LogonUserResultDto()
            {
                Token = token
            };
        }

        public void ValidateToken(ValidateTokenDto tokenParams)
        {
            if (tokenParams == null)
            {
                throw new ArgumentNullException(nameof(tokenParams));
            }

            _logger.Debug("Валидирую токен пользователя {0}.", tokenParams.Login);

            var user = _userRepository.FindByLogin(tokenParams.Login);
            user.ValidateToken(tokenParams.Token);

            _logger.Info("Токен пользователя {0} валиден.", tokenParams.Login);
        }
    }
}
