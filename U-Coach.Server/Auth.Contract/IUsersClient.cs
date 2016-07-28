using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
    /// <summary>
    /// Интерфейс доступа к пользователям
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Создать нового пользователя и вернуть его идентификатор.
        /// </summary>
        TokenDto Create(UserDto password);

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует его.
        /// </summary>
        /// <returns>Токен аутентификации</returns>
        TokenDto Logon(string login, PasswordDto password);

        /// <summary>
        /// Проверяет токен пользователя.
        /// </summary>
        /// <param name="tokenDto">Токен пользователя</param>
        void ValidateToken(TokenDto token);
    }
}
