using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Регистрация нового пользователя из системы Facebook.
        /// Операция идемпотентная.
        /// </summary>
        void RegisterFacebookUser(FacebookConnectionDto facebookConnection);
    }
}
