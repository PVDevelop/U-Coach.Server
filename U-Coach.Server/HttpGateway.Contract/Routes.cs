namespace PVDevelop.UCoach.Server.HttpGateway.Contract
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
        public const string UCOACH_LOGON = "api/ucoach/logon";

        /// <summary>
        /// Команда на логаут текущего пользователя
        /// </summary>
        public const string LOGOUT = "api/current_user/logout";

        /// <summary>
        /// Адрес ресурса инфы текущего пользователя
        /// </summary>
        public const string USER_INFO = "api/current_user";
    }
}
