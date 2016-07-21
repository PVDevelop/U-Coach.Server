
namespace PVDevelop.UCoach.Server.Role.Domain
{
    public interface IUserService
    {
        /// <summary>
        /// Регистрация нового пользователя из внешней системы аутентифакции и возвращает токен
        /// </summary>
        Token RegisterUserToken(UserId userId, AuthSystemToken authToken);

        /// <summary>
        /// Возвращает пользователя по токену
        /// </summary>
        User GetUserByToken(TokenId tokenId);
    }
}
