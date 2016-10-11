using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    /// <summary>
    /// Идентификатор токена
    /// </summary>
    public class TokenId
    {
        /// <summary>
        /// Ключ токена
        /// </summary>
        public string Token { get; private set; }

        public TokenId(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            Token = token;
        }

        public override int GetHashCode()
        {
            return
                Token.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var tokenId = obj as TokenId;
            if (tokenId == null)
            {
                return false;
            }

            return
                Token == tokenId.Token;
        }
    }
}
