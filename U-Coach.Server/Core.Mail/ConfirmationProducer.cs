using System;
using System.Net;
using System.Net.Mail;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Auth.Mail;
using System.Resources;

namespace PVDevelop.UCoach.Server.Auth.Mail
{
    public class EmailConfirmationProducer : IConfirmationProducer
    {
        private readonly ISettingsProvider<IEmailProducerSettings> _settingsProvider;

        public EmailConfirmationProducer(ISettingsProvider<IEmailProducerSettings> settingsProvider)
        {
            if(settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            _settingsProvider = settingsProvider;
        }

        public void Produce(string address, string key)
        {
            var settings = _settingsProvider.Settings;
            using (var mail = new MailMessage(
                settings.SenderAddress,
                address,
                Properties.Resources.NewUserConfirmationHeader,
                string.Format(Properties.Resources.NewUserConfirmationBody, key)))
            {
                var client = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
                {
                    EnableSsl = settings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(settings.UserName, settings.Password),
                };

                client.Send(mail);
            }
        }
    }
}
