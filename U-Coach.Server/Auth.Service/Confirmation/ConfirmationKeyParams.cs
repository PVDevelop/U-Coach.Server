namespace PVDevelop.UCoach.Server.Auth.Service
{
    /// <summary>
    /// Параметры доставки ключа подтверждения создания спортсмена.
    /// </summary>
    public class ConfirmationKeyParams
    {
        /// <summary>
        /// Адрес доставки.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Ключ подтверждения.
        /// </summary>
        public string ConfirmationKey { get; set; }
    }
}
