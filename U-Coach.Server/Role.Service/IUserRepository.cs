using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IUserRepository
    {
        bool TryGet(UserId id, out IUser user);

        void Insert(IUser user);

        void Update(IUser user);
    }
}
