using DevOne.Security.Cryptography.BCrypt;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Domain;
using PVDevelop.UCoach.Server.Timing;
using System;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Доменная модель - пользователь
    /// </summary>
    public class User : AAggregateRoot
    {
        /// <summary>
        /// Логин пользователя. Уникален в БД.
        /// </summary>
        public string Login { get; internal set; }

        /// <summary>
        /// Время создания пользователя
        /// </summary>
        public DateTime CreationTime { get; internal set; }

        /// <summary>
        /// Пароль пользователя. Закодирован.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Возвращает true, если пользователь залогинен
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        internal User() { }

        /// <summary>
        /// Кодирует и устанавливает указанный пароль
        /// </summary>
        /// <param name="plainPassword">Не кодированный пароль</param>
        internal void SetPassword(string plainPassword)
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

            var salt = BCryptHelper.GenerateSalt();
            return BCryptHelper.HashPassword(Password, salt);
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
