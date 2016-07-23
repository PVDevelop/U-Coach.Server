namespace PVDevelop.UCoach.Server.Auth.Domain
{
#warning давай без фабрики для агрегатов
    public interface IUserFactory
    {
        User CreateUser(string login, string password);
    }
}
