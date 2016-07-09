namespace PVDevelop.UCoach.Server.Role.Contract
{
    /// <summary>
    /// Результат редиректа на страницу аутентификации по протоколу oauth
    /// </summary>
    public class OAuthRedirectDto
    {
        /// <summary>
        /// Соедржимое редиректа
        /// </summary>
        public string Content { get; set; }
    }
}
