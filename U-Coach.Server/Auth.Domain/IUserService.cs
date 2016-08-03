namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Интерфейс сервиса взаимодейтсвия с пользователем.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="login">Логин нового пользователя.</param>
        /// <param name="password">Пароль нового пользователя.</param>
        /// <param name="url4Confirm">Url для отправки пользовательского подтверждения</param>
        /// <returns>Токен доступа</returns>
        void CreateUser(string login, string password, string url4Confirm);

        /// <summary>
        /// Проверяет параметры пользователя и если они верны, аутентифицирует его.
        /// </summary>
        /// <param name="login">Параметры аутентификацити, логин</param>
        /// <param name="password">Параметры аутентификацити, пароль</param>
        /// <returns>Токен аутентификации</returns>
        Token Logon(string login, string password);

        /// <summary>
        /// Проверка ключа доступа
        /// </summary>
        /// <param name="tokenParams">Токен пользователя</param>
        void ValidateToken(string token);

        /// <summary>
        /// Подтверждение пользователя
        /// <param name="key">ключ подтверждения</param>
        /// </summary>
        Token Confirm(string key);

        /// <summary>
        /// Повторно отправляет код подтверждения для указанного пользователя
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <param name="url4Confirm">Url для отправки пользовательского подтверждения</param>
        void ResendConfirmation(string login, string url4Confirm);
    }
}
