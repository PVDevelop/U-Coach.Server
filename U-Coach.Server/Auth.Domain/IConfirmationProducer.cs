namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Осуществляет доставку ключа подтверждения.
    /// </summary>
    public interface IConfirmationProducer
    {
        /// <summary>
        /// Доставляет ключ.
        /// </summary>
        /// <param name="address">почтовый адресс</param>
        /// <param name="url">ссылка для подтверждения</param>
        void Produce(string address, string url);
    }
}
