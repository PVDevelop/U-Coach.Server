using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class User
    {
        public UserId Id { get; private set; }

        public User(UserId id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }
    }
}
