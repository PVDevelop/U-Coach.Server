namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public interface IConfirmationRepository
    {
        /// <summary>
        /// Вставить новоe подтверждение.
        /// </summary>
        /// <param name="confirmation">Подтверждение</param>
        void Replace(Confirmation confirmation);

        /// <summary>
        /// Поиск объкста подтверждения по его ключу.
        /// </summary>
        /// <param name="key">Ключ подтверждения.</param>
        Confirmation FindByConfirmation(string key);

        /// <summary>
        /// Удаляет подтверждение по ключу
        /// </summary>
        /// <param name="key"></param>
        void Delete(string key);
    }
}
