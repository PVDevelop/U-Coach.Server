using PVDevelop.UCoach.Server.Exceptions.Auth;
using PVDevelop.UCoach.Server.Mongo;
using System;

namespace PVDevelop.UCoach.Server.AuthService
{
    public class UserService : IUserService
    {
        internal const string USER_COLLECITION_NAME = "User";

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
            _repository.Insert(USER_COLLECITION_NAME, user);
        }

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует пользователя
        /// </summary>
        /// <param name="userParams">Параметры аутентификацити</param>
        public void Logon(LogonUserParams userParams)
        {
            if (userParams == null)
            {
                throw new ArgumentNullException("userParams");
            }

            var user = _repository.Find(USER_COLLECITION_NAME, u => u.Login == userParams.Login);
            user.Logon(userParams.Password);
            _repository.Replace(USER_COLLECITION_NAME, user);
        }

        /// <summary>
        /// Выводит пользователя из системы
        /// </summary>
        /// <param name="userParams">Параметры пользователя</param>
        public void Logout(LogoutUserParams userParams)
        {
            if (userParams == null)
            {
                throw new ArgumentNullException("userParams");
            }

            var user = _repository.Find(USER_COLLECITION_NAME, u => u.Login == userParams.Login);
            user.Logout();
            _repository.Replace(USER_COLLECITION_NAME, user);
        }
    }
}
