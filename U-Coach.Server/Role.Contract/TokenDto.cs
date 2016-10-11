using System;
using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    /// <summary>
    /// Токена пользователя системы авторизации
    /// </summary>
    public class TokenDto
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; private set; }

        public TokenDto(string token, DateTime expiration)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            if (expiration == default(DateTime))
            {
                throw new ArgumentException("Not set", nameof(expiration));
            }
            if (expiration.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Not UTC", nameof(expiration));
            }

            Token = token;
            Expiration = expiration;
        }
    }
}
