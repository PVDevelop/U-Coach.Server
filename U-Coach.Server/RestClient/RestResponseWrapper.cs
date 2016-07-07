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
                throw new ArgumentNullException(nameof(response));
            }
            _response = response;
        }

        public string GetContentOrThrow()
        {
            if (_response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new RestExecutionException(this);
            }
            return _response.Content;
        }

        public void CheckResult()
        {
            if (_response.StatusCode != System.Net.HttpStatusCode.OK &&
                _response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new RestExecutionException(this);
            }
        }
    }
}
