using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;

namespace PVDevelop.UCoach.Server.WebApi
{
    public static class HttpResponseMessageExtensions
    {
        public static T ToJson<T>(
            this HttpResponseMessage message)
        {
            var result = message.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static void CopyCookies(
            this HttpResponseMessage message, 
            HttpResponseBase target)
        {
            IEnumerable<string> headerCookies;
            if (message.Headers.TryGetValues("Set-Cookie", out headerCookies))
            {
                // копируем cookie в наш ответ
                foreach (var headerCookie in headerCookies)
                {
                    CookieHeaderValue cookieHeaderValue;
                    if (CookieHeaderValue.TryParse(headerCookie, out cookieHeaderValue))
                    {
                        foreach (var cookie in cookieHeaderValue.Cookies)
                        {
                            var httpCookie = new HttpCookie(cookie.Name, cookie.Value)
                            {
                                Domain = cookieHeaderValue.Domain,
                                HttpOnly = cookieHeaderValue.HttpOnly,
                                Path = cookieHeaderValue.Path,
                                Secure = cookieHeaderValue.Secure
                            };

                            if (cookieHeaderValue.Expires.HasValue)
                            {
                                httpCookie.Expires = cookieHeaderValue.Expires.Value.UtcDateTime;
                            }

                            target.SetCookie(httpCookie);
                        }
                    }
                }
            }
        }
    }
}
