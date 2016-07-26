namespace PVDevelop.UCoach.Server.Role.Contract
{
    public static class Routes
    {
        /// <summary>
        /// Адрес ресурса авторизации пользователя facebook
        /// </summary>
        public const string FACEBOOK_REDIRECT_URI = "api/facebook/authorization_uri";

        /// <summary>
        /// Адрес токена пользователя facebook
        /// </summary>
        public const string FACEBOOK_TOKEN = "api/facebook/token";

        /// <summary>
        /// Адрес токена пользователя UCoach 
        /// </summary>
        public const string UCOACH_TOKEN = "api/ucoach/token";

        /// <summary>
        /// Адрес пользователя по токену
        /// </summary>
        public const string USER_INFO = "api/users/{token}";

        /// <summary>
        /// PUT - запрос на валидацию токена
        /// </summary>
        public const string VALIDATE_TOKEN = "api/tokens/{token}/validate";

        /// <summary>
        /// DELETE - запрос на удаление токена
        /// </summary>
        public const string DELETE_TOKEN = "api/tokens/{token}";
    }
}
