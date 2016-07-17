using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class Token
    {
        /// <summary>
        /// Идентификатор токена
        /// </summary>
        public TokenId Id { get; private set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит токен
        /// </summary>
        public UserId UserId { get; private set; }

        /// <summary>
        /// Параметры токена внешней системы
        /// </summary>
        public AuthTokenParams TokenParams { get; private set; }

        public Token(
            TokenId id,
            UserId userId,
            AuthTokenParams tokenParams)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (tokenParams == null)
            {
                throw new ArgumentNullException(nameof(tokenParams));
            }

            Id = id;
            UserId = userId;
            TokenParams = tokenParams;
        }
    }
}
