using System;

namespace PVDevelop.UCoach.Server.RestClient
{
    public class RestExecutionException : Exception
    {
        public IRestResponse Response { get; private set; }

        public RestExecutionException(IRestResponse response)
        {
            if(response == null)
            {
                throw new ArgumentNullException("response");
            }
            Response = response;
        }
    }
}
