namespace PVDevelop.UCoach.Server.Auth.Contract
{
    /// <summary>
    /// Интерфейс доступа к пользователям
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Создать нового пользователя и вернуть его идентификатор.
        /// </summary>
        /// <param name="userDto">Параметры создания.</param>
        string Create(CreateUserDto userDto);
    }
}
