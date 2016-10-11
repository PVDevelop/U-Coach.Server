using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class User
    {
        public UserId Id { get; private set; }

        public AuthUserId AuthUserId {get;private set;}

        public User(UserId id, AuthUserId authUserId)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if(authUserId == null)
            {
                throw new ArgumentNullException(nameof(authUserId));
            }

            Id = id;
            AuthUserId = authUserId;
        }
    }
}
