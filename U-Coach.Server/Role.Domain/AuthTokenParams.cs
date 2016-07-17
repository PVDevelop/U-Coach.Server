using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    /// <summary>
    /// Токен внешней системы аутентификации
    /// </summary>
    public class AuthTokenParams
    {
        /// <summary>
        /// Имя внешней системы
        /// </summary>
        public string AuthSystemName { get; private set; }

        /// <summary>
        /// Идентификатор пользователя во внешней системе
        /// </summary>
        public string AuthUserId { get; private set; }

        /// <summary>
        /// Токен пользователя во внешней системе
        /// </summary>
        public string Token { get; private set; }

        public AuthTokenParams(
            string authSystemName,
            string authUserId,
            string token)
        {
            if(authSystemName == null)
            {
                throw new ArgumentNullException(nameof(authSystemName));
            }
            if (authUserId == null)
            {
                throw new ArgumentNullException(nameof(authUserId));
            }
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            AuthSystemName = authSystemName;
            AuthUserId = authUserId;
            Token = token;
        }
    }
}
