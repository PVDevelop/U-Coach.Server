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
        void Create(UserDto password);

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

        /// <summary>
        /// Операция подтверждение пользователя (ввод ключа проверки).
        /// </summary>
        /// <param name="confirmation">Код подтверждения</param>
        void Confirm(ConfirmationDto confirmation);

        /// <summary>
        /// Операция повторного отправления кода подтверждения.
        /// </summary>
        void ResendConfirmation(string login);
    }
}
