namespace PVDevelop.UCoach.Server.Role.Domain.AuthTokenValidation
{
    public interface IAuthTokenValidator
    {
        void Validate(AuthSystemToken authSystemToken);
    }
}
