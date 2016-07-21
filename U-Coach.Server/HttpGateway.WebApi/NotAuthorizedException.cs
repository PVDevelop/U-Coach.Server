using System;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message) : base(message) { }
    }
}
