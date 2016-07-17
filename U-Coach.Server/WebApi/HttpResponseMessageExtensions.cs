using System.Net.Http;
using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.WebApi
{
    public static class HttpResponseMessageExtensions
    {
        public static T ToJson<T>(this HttpResponseMessage message)
        {
            message.EnsureSuccessStatusCode();
            var result = message.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
