using System;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.RestClient
{
    public class RestTokensClient : ITokensClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public RestTokensClient(IRestClientFactory restClientFactory)
        {
            if(restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }

            _restClientFactory = restClientFactory;
        }

        public void Delete(string token)
        {
            _restClientFactory.
                CreateDelete(Routes.DELETE_TOKEN, token).
                Execute().
                CheckGetResult();
        }
    }
}
