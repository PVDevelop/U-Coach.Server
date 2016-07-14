namespace PVDevelop.UCoach.Server.Role.Contract
{
    public static class Routes
    {
        /// <summary>
        /// Адрес ресурса авторизации пользователя facebook
        /// </summary>
        public const string FACEBOOK_REDIRECT_URI = "api/facebook/authorization";

        /// <summary>
        /// Адрес ресурас профиля пользователя facebook
        /// </summary>
        public const string FACEBOOK_USER_PROFILE = "api/facebook/connection";
    }
}
