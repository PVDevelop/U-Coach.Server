using System;

namespace PVDevelop.UCoach.Server.Core.Domain
{
    public static class SportsmanConfirmationFactory
    {
        public static SportsmanConfirmation CreateSportsmanConfirmation(
            string authUserId,
            string confirmationKey)
        {
            if(authUserId == null)
            {
                throw new ArgumentNullException("authUserId");
            }
            if (confirmationKey == null)
            {
                throw new ArgumentNullException("confirmationKey");
            }

            return new SportsmanConfirmation
            {
                AuthSystem = SportsmanConfirmationAuthSystem.UCoach,
                ConfirmationKey = confirmationKey,
                AuthUserId = authUserId
            };
        }
    }
}
