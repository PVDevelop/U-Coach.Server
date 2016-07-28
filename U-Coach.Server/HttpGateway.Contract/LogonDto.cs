using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.HttpGateway.Contract
{
    public class LogonDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
