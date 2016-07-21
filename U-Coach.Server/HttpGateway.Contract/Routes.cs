namespace PVDevelop.UCoach.Server.HttpGateway.Contract
{
    public static class Routes
    {        
        /// <summary>
        /// Адрес ресурса авторизации пользователя facebook
        /// </summary>
        public const string FACEBOOK_REDIRECT_URI = "api/facebook/authorization_uri";

        /// <summary>
        /// Адрес ресура с профиля пользователя facebook
        /// </summary>
        public const string FACEBOOK_TOKEN = "api/facebook/token";

        /// <summary>
        /// Команда на логаут текущего пользователя
        /// </summary>
        public const string LOGOUT = "api/logout";

        /// <summary>
        /// GET-запрос на получение пользователя по токену
        /// </summary>
        public const string USER_INFO = "api/users/{token}";
    }
}
