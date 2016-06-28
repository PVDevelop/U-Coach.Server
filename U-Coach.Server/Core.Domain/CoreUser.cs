using PVDevelop.UCoach.Server.Core.Domain.Exceptions;
using PVDevelop.UCoach.Server.Domain;
using System;

namespace PVDevelop.UCoach.Server.Core.Domain
{
    internal class CoreUser : 
        AAggregateRoot,
        ICoreUser
    {
        public string AuthId { get; internal set; }

        public CoreUserAuthSystem AuthSystem { get; internal set; }

        public string ConfirmationKey { get; internal set; }

        public CoreUserState State { get; private set; }

        public void Confirm(string confirmationKey)
        {
            if(confirmationKey != ConfirmationKey)
            {
                throw new InvalidConfirmationKeyException();
            }

            State = CoreUserState.Confirmed;
        }
    }
}
