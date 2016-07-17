using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Регистрация нового пользователя из внешней системы аутентифакции и возвращает токен
        /// </summary>
        TokenId RegisterUserToken(AuthTokenParams authTokenParams);
    }
}
