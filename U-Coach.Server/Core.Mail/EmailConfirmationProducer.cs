using PVDevelop.UCoach.Server.Core.Service;
using System;
using System.Net;
using System.Net.Mail;
using PVDevelop.UCoach.Server.Configuration;

namespace PVDevelop.UCoach.Server.Core.Mail
{
    public class EmailConfirmationProducer : ISportsmanConfirmationProducer
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

        public void Produce(ProduceConfirmationKeyParams user)
        {
            var settings = _settingsProvider.Settings;
            using (var mail = new MailMessage(
                settings.SenderAddress, 
                user.Address, 
                Properties.Resources.NewUserConfirmationHeader,
                string.Format(Properties.Resources.NewUserConfirmationBody, user.ConfirmationKey)))
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
