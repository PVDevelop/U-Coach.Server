using PVDevelop.UCoach.Server.Core.Domain;

namespace PVDevelop.UCoach.Server.Core.Service
{
    /// <summary>
    /// Осуществляет доставку ключа подтверждения спортсмена.
    /// </summary>
    public interface ISportsmanConfirmationProducer
    {
        /// <summary>
        /// Доставляет ключ спортсмена.
        /// </summary>
        /// <param name="user">Параметры доставки ключа.</param>
        void Produce(ProduceConfirmationKeyParams user);
    }
}
