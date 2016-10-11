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
    public class User 
    {
        /// <summary>
        /// Идентификатор Id в системе. Уникален в БД.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Логин пользователя = почта. Уникален в БД.
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// Время создания пользователя
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// Пароль пользователя. Закодирован.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Подтвердил ли пользователь аккаунт
        /// </summary>
        public ConfirmationStatus ConfirmationStatus { get; private set; }

        public User(
            string id,
            DateTime creationTime)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.ConfirmationStatus = ConfirmationStatus.Unconfirmed;
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
        /// Проверка пароля.
        /// </summary>
        /// <param name="plainPassword"></param>
        public void CheckPassword(string plainPassword)
        {
            if (!BCryptHelper.CheckPassword(plainPassword, Password))
            {
                throw new InvalidPasswordException();
            }
        }

        /// <summary>
        /// Установка статуса подтверждение
        /// </summary>
        public void Confirm()
        {
            this.ConfirmationStatus = ConfirmationStatus.Confirmed;
        }
    }
}
