using System;
using PVDevelop.UCoach.Server.Configuration;

namespace PVDevelop.UCoach.Server.RestClient
{
    public class RestClientFactory : IRestClientFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public RestClientFactory(IConnectionStringProvider connectionStringProvider)
        {
            if(connectionStringProvider == null)
            {
                throw new ArgumentNullException(nameof(connectionStringProvider));
            }
            _connectionStringProvider = connectionStringProvider;
        }

        public IRestClient CreateGet(string resource, params string[] segments)
        {
            return Create(resource, RestSharp.Method.GET, segments);
        }

        public IRestClient CreatePost(string resource, params string[] segments)
        {
            return Create(resource, RestSharp.Method.POST, segments);
        }

        public IRestClient CreatePut(string resource, params string[] segments)
        {
            return Create(resource, RestSharp.Method.PUT, segments);
        }

        private IRestClient Create(
            string resource, 
            RestSharp.Method method,
            params string[] segments)
        {
            var client = new RestSharp.RestClient(_connectionStringProvider.ConnectionString);

            var uri = RestHelper.FormatUri(resource, segments);
            var request = new RestSharp.RestRequest(uri, method);

            return new RestClientWrapper(client, request);
        }
    }
}
