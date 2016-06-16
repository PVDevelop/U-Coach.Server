namespace PVDevelop.UCoach.Server.AuthService
{
    public class LogoutByPasswordUserParams
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
