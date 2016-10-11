namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public static class Routes
    {
        public const string LOGON_USER = "api/users/{login}/logon";
        public const string RESEND_CONFIRM = "api/users/{login}/sendconfirmation";

        public const string CREATE_USER = "api/users";
        public const string VALIDATE_USER_TOKEN = "api/users/validate";
        public const string CONFIRM_USER = "api/users/confirm";
    }
}
