using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class FacebookRedirectDto
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
