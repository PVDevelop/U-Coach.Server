namespace PVDevelop.UCoach.Server.Role.Domain.Validator
{
    public interface IAuthTokenValidator
    {
        void Validate(AuthSystemToken authSystemToken);
    }
}
