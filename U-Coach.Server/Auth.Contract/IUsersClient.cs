using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
#warning агрегаты не должны возвращаться из клиента, надо делать TokenDto
    /// <summary>
    /// Интерфейс доступа к пользователям
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Создать нового пользователя и вернуть его идентификатор.
        /// </summary>
        Token Create(string login, string password);

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует его.
        /// </summary>
        /// <returns>Токен аутентификации</returns>
        Token Logon(string login, string password);

        /// <summary>
        /// Проверяет токен пользователя.
        /// </summary>
        /// <param name="tokenDto">Токен пользователя</param>
        void ValidateToken(string token);
    }
}
