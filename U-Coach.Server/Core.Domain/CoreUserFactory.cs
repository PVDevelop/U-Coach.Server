namespace PVDevelop.UCoach.Server.Core.Domain
{
    public static class CoreUserFactory
    {
        public static ICoreUser CreateUCoachUser(string confirmationKey)
        {
            return new CoreUser
            {
                AuthSystem = CoreUserAuthSystem.UCoach,
                ConfirmationKey = confirmationKey
            };
        }
    }
}
