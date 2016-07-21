using System;
using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class TokenDto
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; private set; }

        public TokenDto(string token, DateTime expiration)
        {
            if(token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            if(expiration == default(DateTime))
            {
                throw new ArgumentException("Expiration not set");
            }

            Token = token;
            Expiration = expiration;
        }
    }
}
