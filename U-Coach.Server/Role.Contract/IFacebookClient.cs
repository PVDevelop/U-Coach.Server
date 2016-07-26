namespace PVDevelop.UCoach.Server.Role.Contract
{
    public interface IFacebookClient
    {
        /// <summary>
        /// Возвращает Uri к странице авторизации
        /// </summary>
        FacebookRedirectDto GetAuthPageUri(string redirectUri);

        /// <summary>
        /// Обменивает код доступа на токен авторизации в системе
        /// </summary>
        /// <param name="code">Код доступа</param>
        TokenDto ExchangeCodeByToken(string code, string redirectUri);
    }
}
