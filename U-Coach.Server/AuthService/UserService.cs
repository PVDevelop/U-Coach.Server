using PVDevelop.UCoach.Server.Exceptions.Auth;
using PVDevelop.UCoach.Server.Mongo;
using System;

namespace PVDevelop.UCoach.Server.AuthService
{
    public class UserService : IUserService
    {
        private readonly IMongoRepository<User> _repository;

        public UserService(IMongoRepository<User> repository)
        {
            if(repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
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

            var user = new User(userParams.Login);
            user.SetPassword(userParams.Password);
            _repository.Insert(user);
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

            var user = _repository.Find(u => u.Login == userParams.Login);
            var token = user.Logon(userParams.Password);
            _repository.Replace(user);

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

            var user = _repository.Find(u => u.Login == userParams.Login);
            user.Logout(userParams.Password);
            _repository.Replace(user);
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

            var user = _repository.Find(u => u.Login == tokenParams.Login);
            user.ValidateToken(tokenParams.Token);
        }
    }
}
