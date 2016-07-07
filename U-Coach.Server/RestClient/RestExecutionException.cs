using System;

namespace PVDevelop.UCoach.Server.RestClient
{
    public class RestExecutionException : Exception
    {
        public RestExecutionException(string message) : 
            base(message)
        {
        }
    }
}
