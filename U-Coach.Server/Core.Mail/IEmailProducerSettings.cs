namespace PVDevelop.UCoach.Server.Core.Mail
{
    /// <summary>
    /// Настройки отправителя почты
    /// </summary>
    public interface IEmailProducerSettings
    {
        /// <summary>
        /// SMTP адрес хоста
        /// </summary>
        string SmtpHost { get; }

        /// <summary>
        /// Порт
        /// </summary>
        int SmtpPort { get; }

        /// <summary>
        /// Логин
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Пароль
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Адрес отправителя
        /// </summary>
        string SenderAddress { get; }
    }
}
