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

        public IRestClient CreatePost(string resource)
        {
            var client = new RestSharp.RestClient(_connectionStringProvider.ConnectionString);
            var request = new RestSharp.RestRequest(resource, RestSharp.Method.POST);
            return new RestClientWrapper(client, request);
        }
    }
}
