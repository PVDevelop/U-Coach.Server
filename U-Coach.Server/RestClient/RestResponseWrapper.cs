using System;
using Newtonsoft.Json;

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

        public T GetContent<T>()
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(_response.Content);
        }

        public IRestResponse CheckPostResult()
        {
            if (_response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new RestExecutionException(_response.Content);
            }
            return this;
        }
    }
}
