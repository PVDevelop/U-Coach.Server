using System;
using PVDevelop.UCoach.Server.Core.Mail;

namespace PVDevelop.UCoach.Server.UserManagement
{
#warning убрать fake
    public class FakeEmailProducerSettings : IEmailProducerSettings
    {
        public bool EnableSsl
        {
            get
            {
                return true;
            }
        }

        public string Password
        {
            get
            {
                return "tkvXp7IvXlUEo9N7EBU7";
            }
        }

        public string SenderAddress
        {
            get
            {
                return "PVDevelop@yandex.ru";
            }
        }

        public string SmtpHost
        {
            get
            {
                return "smtp.yandex.ru";
            }
        }

        public int SmtpPort
        {
            get
            {
                return 25;
            }
        }

        public string UserName
        {
            get
            {
                return "PVDevelop@yandex.ru";
            }
        }
    }
}
