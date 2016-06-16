using DevOne.Security.Cryptography.BCrypt;
using PVDevelop.UCoach.Server.Exceptions.Auth;
using PVDevelop.UCoach.Server.Mongo;
using System;
using Utilities;

namespace PVDevelop.UCoach.Server.AuthService
{
    public class User : IHaveId
    {
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Логин пользователя. Уникален в БД.
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// Пароль пользователя. Закодирован.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Возвращает true, если пользователь залогинен
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        /// Время последней аутентификации пользователя
        /// </summary>
        public DateTime LastAuthenticationTime { get; private set; }

        /// <summary>
        /// Время создания пользователя
        /// </summary>
        public DateTime CreationTime { get; private set; }

        internal User(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new LoginNotSetException();
            }

            Login = login;
            CreationTime = UtcTime.UtcNow;
        }

        /// <summary>
        /// Кодирует и устанавливает указанный пароль
        /// </summary>
        /// <param name="plainPassword">Не кодированный пароль</param>
        public void SetPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
            {
                throw new InvalidPasswordFormatException();
            }

            var salt = BCryptHelper.GenerateSalt();
            var password = BCryptHelper.HashPassword(plainPassword, salt);
            Password = password;
        }

        /// <summary>
        /// Проверяет пароль и если задан правильно, то делает пользователя залогиненым
        /// </summary>
        /// <param name="plainPassword">Не кодированный пароль</param>
        public void Logon(string plainPassword)
        {
            bool valid = BCryptHelper.CheckPassword(plainPassword, Password);
            if(!valid)
            {
                throw new InvalidPasswordException();
            }

            IsLoggedIn = true;
            LastAuthenticationTime = UtcTime.UtcNow;
        }

        /// <summary>
        /// Выводит пользователся из системы
        /// </summary>
        public void Logout()
        {
            IsLoggedIn = false;
        }
    }
}
