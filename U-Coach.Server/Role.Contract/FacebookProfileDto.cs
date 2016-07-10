using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class FacebookProfileDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
