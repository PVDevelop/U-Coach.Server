namespace PVDevelop.UCoach.Server.Core.Mail
{
    public interface IEmailProducerSettings
    {
        bool EnableSsl { get; }
        string Password { get; }
        string SenderAddress { get; }
        string SmtpHost { get; }
        int SmtpPort { get; }
        string UserName { get; }
    }
}
