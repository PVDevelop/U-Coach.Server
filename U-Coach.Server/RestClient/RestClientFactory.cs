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

        public IRestClient CreatePost(string resource, params string[] segments)
        {
            var client = new RestSharp.RestClient(_connectionStringProvider.ConnectionString);

            var uri = RestHelper.FormatUri(resource, segments);
            var request = new RestSharp.RestRequest(uri, RestSharp.Method.POST);

            return new RestClientWrapper(client, request);
        }
    }
}
