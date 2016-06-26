namespace PVDevelop.UCoach.Server.Auth.Service
{
    /// <summary>
    /// Интерфейс сервиса взаимодейтсвия с пользователем.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="userParams">Логин/пароль нового пользователя.</param>
        void Create(CreateUserParams userParams);

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует его.
        /// </summary>
        /// <param name="userParams">Параметры аутентификацити</param>
        /// <returns>Токен аутентификации</returns>
        string Logon(LogonUserParams userParams);

        /// <summary>
        /// Логаут пользователя из системы.
        /// </summary>
        /// <param name="userParams">Параметры пользователя</param>
        void LogoutByPassword(LogoutByPasswordUserParams userParams);

        /// <summary>
        /// Проверяет токен пользователя.
        /// </summary>
        /// <param name="tokenParams">Токен пользователя</param>
        void ValidateToken(ValidateTokenParams tokenParams);
    }
}
