using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public interface IUserRepository
    {
        /// <summary>
        /// Добавление пользователя в бд, если пользователь уже создан кидает исключение
        /// </summary>
        void Insert(User user);

        /// <summary>
        /// Поиск пользователя по Id
        /// </summary>
        /// <returns></returns>
        User FindById(string id);

        /// <summary>
        /// Поиск пользователя по login
        /// </summary>
        /// <returns></returns>
        User FindByLogin(string login);

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        void Update(User user);
    }
}
