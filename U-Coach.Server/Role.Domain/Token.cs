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
        public AuthSystemToken AuthToken { get; private set; }

        /// <summary>
        /// Время окончания действия токена
        /// </summary>
        public DateTime Expiration { get; private set; }

        /// <summary>
        /// Признак того, что токен был удален
        /// </summary>
        public bool IsDeleted { get; private set; }

        public Token(
            TokenId id,
            UserId userId,
            AuthSystemToken authToken,
            DateTime expiration)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }
            if(expiration == default(DateTime))
            {
                throw new ArgumentException("Not set", nameof(expiration));
            }
            if(expiration.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Not UTC", nameof(expiration));
            }

            Id = id;
            UserId = userId;
            AuthToken = authToken;
            Expiration = expiration;
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void Validate(DateTime utcNow)
        {
            if(IsDeleted)
            {
                throw new NotAuthorizedException(string.Format("Token {0} has been removed", Id));
            }
            if(utcNow > Expiration)
            {
                throw new NotAuthorizedException(string.Format("Token {0} has been expired", Id));
            }
        }
    }
}
