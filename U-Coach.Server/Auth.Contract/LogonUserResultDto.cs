namespace PVDevelop.UCoach.Server.Auth.Contract
{
    /// <summary>
    /// Результат логона пользователя
    /// </summary>
    public class LogonUserResultDto
    {
        /// <summary>
        /// Токен аутентификации
        /// </summary>
        public string Token { get; set; }
    }
}
