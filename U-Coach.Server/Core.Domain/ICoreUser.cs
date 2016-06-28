namespace PVDevelop.UCoach.Server.Core.Domain
{
    /// <summary>
    /// Агрегат пользователся в контексте ядра
    /// </summary>
    public interface ICoreUser
    {
        /// <summary>
        /// Идентификатор пользователя в системе аутентификации.
        /// </summary>
        string AuthId { get; }

        /// <summary>
        /// Система, в которой создан пользователь.
        /// </summary>
        CoreUserAuthSystem AuthSystem { get; }

        /// <summary>
        /// Состояние пользователя.
        /// </summary>
        CoreUserState State { get; }

        /// <summary>
        /// Возвращает ключ подтверждения создания пользователя.
        /// </summary>
        string ConfirmationKey { get; }

        /// <summary>
        /// Подтверждает создание пользователя.
        /// </summary>
        /// <param name="plainKey">Неодированный ключ.</param>
        void Confirm(string plainKey);
    }
}
