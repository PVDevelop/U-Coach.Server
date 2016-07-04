namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public class LogonUserDto
    {        
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя. Не закодирован.
        /// </summary>
        public string Password { get; set; }
    }
}
