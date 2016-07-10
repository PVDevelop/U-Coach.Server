using System;
using System.Configuration;

namespace PVDevelop.UCoach.Server.Role.Service
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

        [ConfigurationProperty("uriRedirectCode")]
        public string UriRedirectToCode
        {
            get
            {
                return (string)base["uriRedirectCode"];
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
