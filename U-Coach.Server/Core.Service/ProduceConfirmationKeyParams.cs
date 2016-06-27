namespace PVDevelop.UCoach.Server.Core.Service
{
    /// <summary>
    /// Параметры доставки ключа подтверждения создания пользователя.
    /// </summary>
    public class ProduceConfirmationKeyParams
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
