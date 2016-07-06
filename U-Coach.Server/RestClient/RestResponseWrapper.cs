using System;

namespace PVDevelop.UCoach.Server.RestClient
{
    public class RestResponseWrapper : IRestResponse
    {
        private readonly RestSharp.IRestResponse _response;

        public string Content
        {
            get
            {
                return _response.Content;
            }
        }

        public HttpStatusCode Status
        {
            get
            {
                return (HttpStatusCode)_response.StatusCode;
            }
        }

        public RestResponseWrapper(RestSharp.IRestResponse response)
        {
            if(response == null)
            {
                throw new ArgumentNullException("response");
            }
            _response = response;
        }
    }
}
