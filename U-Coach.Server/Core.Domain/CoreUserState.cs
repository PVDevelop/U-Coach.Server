namespace PVDevelop.UCoach.Server.Core.Domain
{
    /// <summary>
    /// Состояние пользователя.
    /// </summary>
    public enum CoreUserState
    {
        /// <summary>
        /// Пользователь находится в состоянии ожидания подверждения создания.
        /// </summary>
        WaitingForConfirmation  = 0,

        /// <summary>
        /// Создание пользователя подтверждено.
        /// </summary>
        Confirmed               = 1,
    }
}
