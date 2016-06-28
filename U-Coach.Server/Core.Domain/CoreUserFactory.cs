using System;

namespace PVDevelop.UCoach.Server.Core.Domain
{
    public static class CoreUserFactory
    {
        public static ICoreUser CreateUCoachUser(
            string authId,
            string confirmationKey)
        {
            if(authId == null)
            {
                throw new ArgumentNullException("authId");
            }
            if (confirmationKey == null)
            {
                throw new ArgumentNullException("confirmationKey");
            }

            return new CoreUser
            {
                AuthSystem = CoreUserAuthSystem.UCoach,
                ConfirmationKey = confirmationKey,
                AuthId = authId
            };
        }
    }
}
