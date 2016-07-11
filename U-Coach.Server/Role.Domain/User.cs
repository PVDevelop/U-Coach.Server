using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    internal class User : IUser
    {
        public UserId Id { get; private set; }

        public AuthToken Token { get; set; }

        internal User(UserId id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
            Token = AuthToken.Empty;
        }

        public void SetToken(AuthToken token)
        {
            if(token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            Token = token;
        }
    }
}
