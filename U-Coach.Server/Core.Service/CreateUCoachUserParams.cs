namespace PVDevelop.UCoach.Server.Core.Service
{
    public class CreateUCoachUserParams
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Ключ подтверждения создания пользователя
        /// </summary>
        public string ConfirmationKey { get; set; }

        /// <summary>
        /// Адрес доставки ключа пользователя
        /// </summary>
        public string Address { get; set; }
    }
}
