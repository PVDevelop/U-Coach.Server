using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class AuthToken
    {
        private static readonly AuthToken _empty = new AuthToken();

        /// <summary>
        /// Возвращает пустой токен
        /// </summary>
        public static AuthToken Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Возвращает true, если token не заполнен
        /// </summary>
        public bool IsEmpty
        {
            get { return ReferenceEquals(this, _empty); }
        }

        /// <summary>
        /// Токен авторизации
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Время истечения токена. Если не назначено, то время неизвестно.
        /// </summary>
        public DateTime? ExpirationTime { get; private set; }

        public AuthToken(string token, DateTime? expiratnioTime)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            Token = token;
            ExpirationTime = expiratnioTime;
        }

        private AuthToken() { }

        public override int GetHashCode()
        {
            return Token.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var token = obj as AuthToken;
            if (token == null)
            {
                return false;
            }

            return
                Token == token.Token &&
                ExpirationTime == token.ExpirationTime;
        }
    }
}
