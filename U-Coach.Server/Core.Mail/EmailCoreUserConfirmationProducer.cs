﻿using PVDevelop.UCoach.Server.Core.Service;
using System;
using System.Net;
using System.Net.Mail;

namespace PVDevelop.UCoach.Server.Core.Mail
{
    public class EmailCoreUserConfirmationProducer : ICoreUserConfirmationProducer
    {
        private readonly IEmailProducerSettings _settings;

        public EmailCoreUserConfirmationProducer(IEmailProducerSettings settings)
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
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                };

                client.Send(mail);
            }
        }
    }
}
