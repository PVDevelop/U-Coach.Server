namespace PVDevelop.UCoach.Server.Role.Domain
{
    public interface IUser
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        UserId Id { get; }

        /// <summary>
        /// Токен авторизации
        /// </summary>
        AuthToken Token { get; }

        /// <summary>
        /// Устанавливает токен
        /// </summary>
        void SetToken(AuthToken token);
    }
}
