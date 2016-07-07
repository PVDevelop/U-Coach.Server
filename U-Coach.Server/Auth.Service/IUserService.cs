using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    /// <summary>
    /// Интерфейс сервиса взаимодейтсвия с пользователем.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создать нового пользователя и вернуть его идентификатор.
        /// </summary>
        /// <param name="userParams">Логин/пароль нового пользователя.</param>
        string Create(CreateUserDto userParams);

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует его.
        /// </summary>
        /// <param name="userParams">Параметры аутентификацити</param>
        /// <returns>Токен аутентификации</returns>
        string Logon(LogonUserDto userParams);

        /// <summary>
        /// Проверяет токен пользователя.
        /// </summary>
        /// <param name="tokenParams">Токен пользователя</param>
        void ValidateToken(ValidateTokenDto tokenParams);
    }
}
