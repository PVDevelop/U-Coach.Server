namespace PVDevelop.UCoach.Server.Role.Contract
{
    public interface IFacebookClient
    {
        /// <summary>
        /// Возвращает url к странице авторизации
        /// </summary>
        /// <returns></returns>
        FacebookRedirectDto GetAuthorizationUrl(string redirectUri);

        /// <summary>
        /// Возвращает профиль пользователя по коду
        /// </summary>
        FacebookConnectionDto GetConnection(string code, string redirectUri);
    }
}
