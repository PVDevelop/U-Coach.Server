namespace PVDevelop.UCoach.Server.Role.Contract
{
    public interface ITokensClient
    {
        void Validate(string token);

        void Delete(string token);
    }
}
