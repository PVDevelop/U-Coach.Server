namespace PVDevelop.UCoach.Server.Role.Contract
{
    public interface IFacebookClient
    {
        /// <summary>
        /// Возвращает url к странице авторизации
        /// </summary>
        /// <returns></returns>
        FacebookRedirectDto GetAuthorizationUrl();

        /// <summary>
        /// Возвращает профиль пользователя по коду
        /// </summary>
        FacebookProfileDto GetProfile(FacebookCodeDto codeDto);
    }
}
