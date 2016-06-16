namespace PVDevelop.UCoach.Server.AuthService
{
    public class CreateUserParams
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
