namespace PVDevelop.UCoach.Server.Auth.Service
{
    /// <summary>
    /// Осуществляет доставку ключа подтверждения спортсмена.
    /// </summary>
    public interface IConfirmationProducer
    {
        /// <summary>
        /// Доставляет ключ спортсмена.
        /// </summary>
        void Produce(string address, string key);
    }
}
