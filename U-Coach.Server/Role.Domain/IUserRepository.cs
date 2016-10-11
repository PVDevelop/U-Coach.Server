namespace PVDevelop.UCoach.Server.Role.Domain
{
    public interface IUserRepository
    {
        bool TryGet(UserId id, out User user);

        bool TryGetByAuthUserId(AuthUserId authUserId, out User user);

        void Insert(User user);
    }
}
