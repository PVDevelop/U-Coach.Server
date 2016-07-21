using System;
using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    /// <summary>
    /// Dto для регистрации пользователя в системе по токену внешней системы
    /// </summary>
    public class AuthUserRegisterDto
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; private set; }

        public AuthUserRegisterDto(string token, DateTime expiration)
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
