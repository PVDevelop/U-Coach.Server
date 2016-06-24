namespace PVDevelop.UCoach.Server.Auth.Service
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
