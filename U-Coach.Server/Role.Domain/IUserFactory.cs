namespace PVDevelop.UCoach.Server.Role.Domain
{
    public interface IUserFactory
    {
        IUser CreateUser(UserId userId);
    }
}
