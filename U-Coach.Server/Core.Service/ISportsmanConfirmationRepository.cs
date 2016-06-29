using PVDevelop.UCoach.Server.Core.Domain;

namespace PVDevelop.UCoach.Server.Core.Service
{
    public interface ISportsmanConfirmationRepository
    {
        /// <summary>
        /// Вставить новоe подтверждение.
        /// </summary>
        /// <param name="confirmation">Подтверждение создание спортсмена.</param>
        void Insert(SportsmanConfirmation confirmation);

        /// <summary>
        /// Находит пользователя по ключу подтверждения.
        /// </summary>
        /// <param name="key">Ключ подтверждения.</param>
        /// <returns></returns>
        SportsmanConfirmation FindByConfirmationKey(string key);
    }
}
