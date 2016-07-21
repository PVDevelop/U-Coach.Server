namespace PVDevelop.UCoach.Server.Role.Contract
{
    public static class Routes
    {
        /// <summary>
        /// PUT-запрос на регистрацию пользователя
        /// </summary>
        public const string REGISTER_USER = "api/systems/{system}/users/{id}";

        /// <summary>
        /// GET-запрос на получение пользователя по токену
        /// </summary>
        public const string USER_INFO = "api/users/{token}";
    }
}
