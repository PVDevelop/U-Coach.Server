using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message) : base(message) { }
    }
}
