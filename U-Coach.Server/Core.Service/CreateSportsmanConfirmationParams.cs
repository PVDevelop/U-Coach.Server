namespace PVDevelop.UCoach.Server.Core.Service
{
    public class CreateSportsmanConfirmationParams
    {
        /// <summary>
        /// Логин спортсмена
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль спортсмена
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Ключ подтверждения создания спортсмена
        /// </summary>
        public string ConfirmationKey { get; set; }

        /// <summary>
        /// Адрес доставки ключа спортсмена
        /// </summary>
        public string Address { get; set; }
    }
}
