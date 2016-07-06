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
        /// <param name="createUserDto">Параметры создания.</param>
        string Create(CreateUserDto createUserDto);

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует его.
        /// </summary>
        /// <param name="logonUserDto">Параметры аутентификацити</param>
        /// <returns>Токен аутентификации</returns>
        string Logon(LogonUserDto logonUserDto);

        /// <summary>
        /// Логаут пользователя из системы.
        /// </summary>
        /// <param name="logoutByPasswordDto">Параметры пользователя</param>
        void LogoutByPassword(LogoutByPasswordUserDto logoutByPasswordDto);

        /// <summary>
        /// Проверяет токен пользователя.
        /// </summary>
        /// <param name="tokenDto">Токен пользователя</param>
        void ValidateToken(ValidateTokenDto tokenDto);
    }
}
