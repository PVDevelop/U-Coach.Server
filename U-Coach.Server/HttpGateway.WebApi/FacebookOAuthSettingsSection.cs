using System.Configuration;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public class FacebookOAuthSettingsSection :
        ConfigurationSection,
        IFacebookOAuthSettings
    {
        [ConfigurationProperty("clientId")]
        public string ClientId
        {
            get
            {
                return (string)base["clientId"];
            }
        }

        [ConfigurationProperty("clientSecret")]
        public string ClientSecret
        {
            get
            {
                return (string)base["clientSecret"];
            }
        }
    }
}
