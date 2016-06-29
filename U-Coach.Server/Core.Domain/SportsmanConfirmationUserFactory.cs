using System;

namespace PVDevelop.UCoach.Server.Core.Domain
{
    public static class SportsmanConfirmationUserFactory
    {
        public static ISportsmanConfirmation CreateSportsmanConfirmation(
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

            return new SportsmanConfirmation
            {
                AuthSystem = SportsmanConfirmationAuthSystem.UCoach,
                ConfirmationKey = confirmationKey,
                AuthId = authId
            };
        }
    }
}
