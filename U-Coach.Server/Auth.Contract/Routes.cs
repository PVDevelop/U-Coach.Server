namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public static class Routes
    {
        public const string CREATE_USER = "api/users";
        public const string LOGON_USER = "api/users/{login}/logon";
        public const string LOGOUT_USER = "api/users/{login}/logout";
        public const string VALIDATE_USER_TOKEN = "api/users/{login}/validate";
    }
}
