namespace PVDevelop.UCoach.Server.Role.Domain.Validator
{
    public interface IAuthTokenValidatorContainer
    {
        IAuthTokenValidator GetValidator(string authSystemName);
    }
}
