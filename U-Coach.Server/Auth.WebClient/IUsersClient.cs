using PVDevelop.UCoach.Server.Auth.WebDto;

namespace PVDevelop.UCoach.Server.Auth.WebClient
{
    /// <summary>
    /// Интерфейс доступа к пользователям
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="userParams">Параметры создания.</param>
        void Create(CreateUserParams userParams);
    }
}
