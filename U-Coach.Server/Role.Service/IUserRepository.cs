using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IUserRepository
    {
        bool Contains(UserId id);

        void Insert(IUser user);

        IUser Get(UserId id);
    }
}
