using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    internal class User : IUser
    {
        public UserId Id { get; private set; }

        internal User(UserId id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }
    }
}
