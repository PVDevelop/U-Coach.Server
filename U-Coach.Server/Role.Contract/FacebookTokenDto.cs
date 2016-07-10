using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class FacebookTokenDto
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresInUnixTime { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string Type { get; set; }
    }
}
