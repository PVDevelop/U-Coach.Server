using System;

namespace PVDevelop.UCoach.Server.RestClient
{
    public class RestResponseWrapper : IRestResponse
    {
        private readonly RestSharp.IRestResponse _response;

        public RestResponseWrapper(RestSharp.IRestResponse response)
        {
            if(response == null)
            {
                throw new ArgumentNullException("response");
            }
            _response = response;
        }

        public string Content
        {
            get
            {
                return _response.Content;
            }
        }
    }
}
