using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public interface IUserRepository
    {
        void Insert(User user);

        User FindByLogin(string login);

        void Update(User user);
    }
}
