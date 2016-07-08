using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class UserFactory : IUserFactory
    {
        public IUser CreateUser(UserId userId)
        {
            return new User(userId);
        }
    }
}
