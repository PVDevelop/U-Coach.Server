namespace PVDevelop.UCoach.Server.Auth.Mail
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
