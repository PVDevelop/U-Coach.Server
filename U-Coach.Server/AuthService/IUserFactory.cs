namespace PVDevelop.UCoach.Server.AuthService
{
    public interface IUserFactory
    {
        User CreateNewUser(string login);
    }
}
