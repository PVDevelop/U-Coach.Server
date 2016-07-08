using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Возвращает строку авторизации пользователя на Facebook.
        /// </summary>
        string GetFacebookAuthorizationPage();

        /// <summary>
        /// Регистрация нового пользователя из системы Facebook. Выполняется после авторизации на сайте Facebook.
        /// </summary>
        void RegisterFacebookUser(RegisterFacebookUserDto facebookUserDto);
    }
}
