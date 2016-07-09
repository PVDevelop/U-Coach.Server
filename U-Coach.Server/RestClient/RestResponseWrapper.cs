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

        public string GetContent()
        {
            return _response.Content;
        }

        public T GetJsonContent<T>()
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(_response.Content);
        }

        public IRestResponse CheckGetResult()
        {
            if(_response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ThrowError();
            }
            return this;
        }

        public IRestResponse CheckPostResult()
        {
            if (_response.StatusCode != System.Net.HttpStatusCode.Created &&
                _response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ThrowError();
            }
            return this;
        }

        public IRestResponse CheckPutResult()
        {
            if (_response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ThrowError();
            }
            return this;
        }

        private void ThrowError()
        {
            if (!string.IsNullOrEmpty(_response.Content))
            {
                throw new RestExecutionException(_response.Content);
            }
            else
            {
                throw new RestExecutionException(string.Format("Http code: {0}", _response.StatusCode));
            }
        }
    }
}
