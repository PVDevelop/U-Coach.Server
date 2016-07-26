using System;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.RestClient
{
    public class RestUCoachAuthClient : IUCoachAuthClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public RestUCoachAuthClient(IRestClientFactory restClientFactory)
        {
            if (restClientFactory == null)
            {
                throw new ArgumentNullException(nameof(restClientFactory));
            }

            _restClientFactory = restClientFactory;
        }

        public TokenDto GetToken(string login, string password)
        {
            return
                _restClientFactory.
                CreateGet(Routes.UCOACH_TOKEN).
                AddParameter("login", login).
                AddParameter("password", password).
                Execute().
                CheckGetResult().
                GetJsonContent<TokenDto>();
        }
    }
}
