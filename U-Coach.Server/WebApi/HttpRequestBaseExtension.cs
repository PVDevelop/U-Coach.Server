using System.Net;
using System.Web;

namespace PVDevelop.UCoach.Server.WebApi
{
    public static class HttpRequestBaseExtension
    {
        public static CookieCollection ToCookieCollection(this HttpRequestBase request)
        {
            var cookies = new CookieCollection();

            foreach(var cookieKey in request.Cookies.AllKeys)
            {
                var requestCookie = request.Cookies[cookieKey];
                var cookie = new Cookie()
                {
                    Name = requestCookie.Name,
                    Value = requestCookie.Value,
                    Domain = request.UrlReferrer.Host
                };
                cookies.Add(cookie);
            }

            return cookies;
        }
    }
}
