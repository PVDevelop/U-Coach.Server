using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IUserRepository
    {
        bool TryGet(UserId id, out User user);

        void Insert(User user);
    }
}
