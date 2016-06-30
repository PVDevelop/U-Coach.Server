using PVDevelop.UCoach.Server.Core.Service;
using System;
using System.Net;
using System.Net.Mail;

namespace PVDevelop.UCoach.Server.Core.Mail
{
    public class EmailConfirmationProducer : ISportsmanConfirmationProducer
    {
        private readonly IEmailProducerSettings _settings;

        public EmailConfirmationProducer(IEmailProducerSettings settings)
        {
            if(settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            _settings = settings;
        }

        public void Produce(ProduceConfirmationKeyParams user)
        {
            var body = string.Format("your key is {0}", user.ConfirmationKey);
            using (var mail = new MailMessage(
                _settings.SenderAddress, 
                user.Address, 
                "hello, beetlewar!", 
                body))
            {
                var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
                {
                    EnableSsl = _settings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                };

#warning не работает отправка
                client.Send(mail);
            }
        }
    }
}
