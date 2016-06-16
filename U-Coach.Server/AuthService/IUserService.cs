namespace PVDevelop.UCoach.Server.AuthService
{
    public interface IUserService
    {
        void Create(CreateUserParams userParams);
        void Logon(LogonUserParams userParams);
    }
}
