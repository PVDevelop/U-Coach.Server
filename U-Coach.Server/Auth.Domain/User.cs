using DevOne.Security.Cryptography.BCrypt;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;
using System;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Доменная модель - пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; private set; }

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

        public User(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new LoginNotSetException();
            }

            Login = login;
            CreationTime = UtcTime.UtcNow;
            Id = Guid.NewGuid();
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
        /// <returns>Токен аутентификации</returns>
        public string Logon(string plainPassword)
        {
            CheckPassword(plainPassword);

            IsLoggedIn = true;
            LastAuthenticationTime = UtcTime.UtcNow;

            var salt = BCryptHelper.GenerateSalt();
            return BCryptHelper.HashPassword(Password, salt);
        }

        /// <summary>
        /// Выводит пользователся из системы
        /// </summary>
        /// <param name="plainPassword">Не кодированный пароль</param>
        public void Logout(string plainPassword)
        {
            CheckPassword(plainPassword);

            IsLoggedIn = false;
        }

        /// <summary>
        /// Проверяет токен. Если не залогинен, кидает NotLoggedInException. Если токен неверный, кидает InvalidTokenException.
        /// </summary>
        public void ValidateToken(string token)
        {
            if(!IsLoggedIn)
            {
                throw new NotLoggedInException();
            }

            try
            {
                if (!BCryptHelper.CheckPassword(Password, token))
                {
                    throw new InvalidTokenException();
                }
            }
            catch(ArgumentException)
            {
                throw new InvalidTokenException();
            }
        }

        private void CheckPassword(string plainPassword)
        {
            bool valid = BCryptHelper.CheckPassword(plainPassword, Password);
            if (!valid)
            {
                throw new InvalidPasswordException();
            }
        }
    }
}
