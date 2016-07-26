namespace PVDevelop.UCoach.Server.Role.Contract
{
    public interface IUsersClient
    {
        /// <summary>
        /// Возвращает инфо пользователя по токену
        /// </summary>
        /// <param name="token">Токен доступа</param>
        UserInfoDto GetUserInfo(
            string token);
    }
}
