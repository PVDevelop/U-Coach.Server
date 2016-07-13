namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IFacebookOAuthSettings
    {
        /// <summary>
        /// Идентификатор приложения - клиента FB
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Кодовое слово приложения - клиента FB
        /// </summary>
        string ClientSecret { get; }
    }
}
