using System;

namespace PVDevelop.UCoach.Server.RestClient
{
    public class RestClientWrapper : IRestClient
    {
        private readonly RestSharp.IRestClient _client;
        private RestSharp.IRestRequest _request;

        public RestClientWrapper(
            RestSharp.IRestClient client, 
            RestSharp.IRestRequest request)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            _client = client;
            _request = request;
        }

        public IRestClient AddBody(object body)
        {
            _request = _request.AddJsonBody(body);
            return this;
        }

        public IRestResponse Execute()
        {
            return new RestResponseWrapper(_client.Execute(_request));
        }
    }
}
