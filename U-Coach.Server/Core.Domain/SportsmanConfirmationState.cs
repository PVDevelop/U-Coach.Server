namespace PVDevelop.UCoach.Server.Core.Domain
{
    /// <summary>
    /// Состояние спортсмена.
    /// </summary>
    public enum SportsmanConfirmationState
    {
        /// <summary>
        /// Спортсмен находится в состоянии ожидания подверждения создания.
        /// </summary>
        WaitingForConfirmation = 0,

        /// <summary>
        /// Создание спортсмена подтверждено.
        /// </summary>
        Confirmed = 1,
    }
}
