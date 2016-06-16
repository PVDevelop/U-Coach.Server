namespace PVDevelop.UCoach.Server.AuthService
{
    public interface IUserService
    {
        void Create(CreateUserParams userParams);
        string Logon(LogonUserParams userParams);
        void LogoutByPassword(LogoutByPasswordUserParams userParams);
        void ValidateToken(ValidateTokenParams tokenParams);
    }
}
