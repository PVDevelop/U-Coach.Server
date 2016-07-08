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
                throw new ArgumentNullException(nameof(client));
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            _client = client;
            _request = request;
        }

        public IRestClient AddBody(object body)
        {
            _request = _request.AddJsonBody(body);
            return this;
        }

        public IRestClient AddParameter(string name, string value)
        {
            _request = _request.AddParameter(name, value);
            return this;
        }

        public IRestResponse Execute()
        {
            return new RestResponseWrapper(_client.Execute(_request));
        }
    }
}
