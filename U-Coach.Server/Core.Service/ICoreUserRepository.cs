using PVDevelop.UCoach.Server.Core.Domain;

namespace PVDevelop.UCoach.Server.Core.Service
{
    public interface ICoreUserRepository
    {
        /// <summary>
        /// Вставить нового пользователя контекста ядра.
        /// </summary>
        /// <param name="user">Пользователь ядра.</param>
        void Insert(ICoreUser user);
    }
}
