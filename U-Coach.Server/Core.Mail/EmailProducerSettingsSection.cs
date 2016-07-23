using System.Configuration;

namespace PVDevelop.UCoach.Server.Auth.Mail
{
    public class EmailProducerSettingsSection :
        ConfigurationSection,
        IEmailProducerSettings
    {
        [ConfigurationProperty("enableSsl")]
        public bool EnableSsl
        {
            get { return (bool)this["enableSsl"]; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
        }

        [ConfigurationProperty("senderAddress")]
        public string SenderAddress
        {
            get { return (string)this["senderAddress"]; }
        }

        [ConfigurationProperty("smtpHost")]
        public string SmtpHost
        {
            get { return (string)this["smtpHost"]; }
        }

        [ConfigurationProperty("smtpPort")]
        public int SmtpPort
        {
            get { return (int)this["smtpPort"]; }
        }

        [ConfigurationProperty("userName")]
        public string UserName
        {
            get { return (string)this["userName"]; }
        }
    }
}
