using PVDevelop.UCoach.Server.Auth.WebDto;

namespace PVDevelop.UCoach.Server.Auth.WebClient
{
    /// <summary>
    /// Интерфейс доступа к пользователям
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Создать нового пользователя и вернуть идентификатор пользователя.
        /// </summary>
        /// <param name="userParams">Параметры создания.</param>
        string Create(CreateUserParams userParams);
    }
}
