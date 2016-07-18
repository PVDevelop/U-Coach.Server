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
        /// <param name="user">Параметры доставки ключа.</param>
        void Produce(ConfirmationKeyParams user);
    }
}
