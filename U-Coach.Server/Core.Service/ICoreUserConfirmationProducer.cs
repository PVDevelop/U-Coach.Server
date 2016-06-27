using PVDevelop.UCoach.Server.Core.Domain;

namespace PVDevelop.UCoach.Server.Core.Service
{
    /// <summary>
    /// Осуществляет доставку ключа подтверждения пользователя.
    /// </summary>
    public interface ICoreUserConfirmationProducer
    {
        /// <summary>
        /// Доставляет ключ пользователя.
        /// </summary>
        /// <param name="user">Параметры доставки ключа.</param>
        void Produce(ProduceConfirmationKeyParams user);
    }
}
