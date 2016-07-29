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
        void Produce(string address, string key);
    }
}
