namespace PVDevelop.UCoach.Server.Role.Domain.AuthTokenValidation
{
    public interface IAuthTokenValidatorContainer
    {
        IAuthTokenValidator GetValidator(string authSystemName);
    }
}
