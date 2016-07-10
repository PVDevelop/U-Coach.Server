namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IFacebookOAuthSettings
    {
        /// <summary>
        /// Идентификатор приложения - клиента FB
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Uri-адресс редиректа получения кода
        /// </summary>
        string UriRedirectToCode { get; }

        /// <summary>
        /// Кодовое слово приложения - клиента FB
        /// </summary>
        string ClientSecret { get; }
    }
}
