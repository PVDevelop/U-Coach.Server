using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.HttpGateway.Contract
{
    public class FacebookLogonDto
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}
