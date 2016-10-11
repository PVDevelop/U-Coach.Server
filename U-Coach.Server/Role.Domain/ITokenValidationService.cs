namespace PVDevelop.UCoach.Server.Role.Domain
{
    public interface ITokenValidationService
    {
        /// <summary>
        /// Валидирует токен и в случае невалидности кидает NotAuthorizedException
        /// </summary>
        void Validate(Token token, string authSystemName);
    }
}
