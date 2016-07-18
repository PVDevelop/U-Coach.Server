using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public interface IConfirmationRepository
    {
        /// <summary>
        /// Вставить новоe подтверждение.
        /// </summary>
        /// <param name="confirmation">Подтверждение создание спортсмена.</param>
        void Obtain(Confirmation confirmation);

        /// <summary>
        /// Находит пользователя по ключу подтверждения.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="key">Ключ подтверждения.</param>
        /// <returns></returns>
        Confirmation FindByConfirmation(string userId, string key);
    }
}
