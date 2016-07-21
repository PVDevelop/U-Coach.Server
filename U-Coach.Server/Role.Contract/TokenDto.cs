using System;
using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class TokenDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }
    }
}
