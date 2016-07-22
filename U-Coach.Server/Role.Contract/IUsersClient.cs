namespace PVDevelop.UCoach.Server.Role.Contract
{
    public interface IUsersClient
    {
        /// <summary>
        /// Регистрирует пользователя в системе и возвращает токен доступа пользователя
        /// </summary>
        /// <param name="authSystemName">Имя системы аутентификации, для которой регистрируется пользователь</param>
        /// <param name="authUserId">Идентификатор пользователя в системе аутентификации</param>
        /// <param name="userRegisterDto">Параметры регистрации пользователя</param>
        TokenDto RegisterUser(
            string authSystemName,
            string authUserId,
            AuthUserRegisterDto userRegisterDto);

        /// <summary>
        /// Возвращает инфо пользователя по токену
        /// </summary>
        /// <param name="token">Токен доступа</param>
        UserInfoDto GetUserInfo(
            string token);
    }
}
