﻿using DevOne.Security.Cryptography.BCrypt;
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
        /// Подтвердил ли пользователь аккаунт
        /// </summary>
        public bool IsConfirmation { get; set; }

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

        public void CheckPassword(string plainPassword)
        {
            bool valid = BCryptHelper.CheckPassword(plainPassword, Password);
            if (!valid)
            {
                throw new InvalidPasswordException();
            }
        }
    }
}
