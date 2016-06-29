using System;

namespace PVDevelop.UCoach.Server.Core.Domain
{
    /// <summary>
    /// Агрегат подтверждения создания спортсмена
    /// </summary>
    public interface ISportsmanConfirmation
    {
        Guid Id { get; }

        /// <summary>
        /// Идентификатор спортсмена в контексте аутентификации.
        /// </summary>
        string AuthUserId { get; }

        /// <summary>
        /// Система, в которой создан спортсмен.
        /// </summary>
        SportsmanConfirmationAuthSystem AuthSystem { get; }

        /// <summary>
        /// Состояние подверждения.
        /// </summary>
        SportsmanConfirmationState State { get; }

        /// <summary>
        /// Возвращает ключ подтверждения создания спортсмена.
        /// </summary>
        string ConfirmationKey { get; }

        /// <summary>
        /// Подтверждает создание спортсмена.
        /// </summary>
        /// <param name="plainKey">Неодированный ключ.</param>
        void Confirm(string plainKey);
    }
}
