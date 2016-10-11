using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class AuthSystemToken
    {
        public string Token { get; private set; }

        public DateTime Expiration { get; private set; }

        public AuthSystemToken(
            string token,
            DateTime expiration)
        {
            if(token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            if(expiration == default(DateTime))
            {
                throw new ArgumentException("Not set", nameof(expiration));
            }
            if(expiration.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Not UTC", nameof(expiration));
            }

            Token = token;
            Expiration = expiration;
        }
    }
}
