using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public class UserValidator : IUserValidator
    {
        public void ValidateLogin(string login)
        {
            if (String.IsNullOrWhiteSpace(login))
            {
                throw new ValidateLoginException();
            }
            if (!Regex.IsMatch(login, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                throw new ValidateLoginException();
            }
        }

        /// <summary>
        /// Проверка пароль имеет буквы разного регистра и цифры, длина пароля от 7 до 15 символов
        /// </summary>
        /// <param name="password"></param>
        public void ValidatePassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new ValidatePasswordException();
            }

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{7,15}$", RegexOptions.IgnoreCase))
            {
                throw new ValidatePasswordException();
            }
        }
    }
}
