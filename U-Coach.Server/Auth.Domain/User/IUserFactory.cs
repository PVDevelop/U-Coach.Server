namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public interface IUserFactory
    {
        User CreateUser(string login, string password);
    }
}
