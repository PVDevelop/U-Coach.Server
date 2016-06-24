using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Доменная модель пользователя
    /// </summary>
    public interface IUser
    {
        Guid Id { get; }

        string Login { get; }

        /// <summary>
        /// Пароль пользователя. Закодирован.
        /// </summary>
        string Password { get; }

        bool IsLoggedIn { get; }

        DateTime LastAuthenticationTime { get; }

        DateTime CreationTime { get; }

        /// <summary>
        /// Кодирует и устанавливает указанный пароль
        /// </summary>
        /// <param name="plainPassword">Не кодированный пароль</param>
        void SetPassword(string plainPassword);

        /// <summary>
        /// Проверяет пароль и если задан правильно, то делает пользователя залогиненым
        /// </summary>
        /// <param name="plainPassword">Не кодированный пароль</param>
        /// <returns>Токен аутентификации</returns>
        string Logon(string plainPassword);

        /// <summary>
        /// Выводит пользователся из системы
        /// </summary>
        /// <param name="plainPassword">Не кодированный пароль</param>
        void Logout(string plainPassword);

        /// <summary>
        /// Проверяет токен. Если не залогинен, кидает NotLoggedInException. Если токен неверный, кидает InvalidTokenException.
        /// </summary>
        void ValidateToken(string token);
    }
}
