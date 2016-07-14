using System;
using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class FacebookConnectionDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("token_expiration")]
        public DateTime TokenExpiration { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
