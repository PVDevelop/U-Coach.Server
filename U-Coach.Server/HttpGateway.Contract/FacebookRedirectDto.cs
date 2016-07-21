using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.HttpGateway.Contract
{
    public class FacebookRedirectDto
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
