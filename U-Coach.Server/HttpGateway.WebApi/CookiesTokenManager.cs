using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public class CookiesTokenManager : ITokenManager
    {
        public const string TOKEN_NAME = "access_token";

        private readonly IUtcTimeProvider _utcTimeProvider;

        public CookiesTokenManager(IUtcTimeProvider utcTimeProvider)
        {
            if(utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }
            _utcTimeProvider = utcTimeProvider;
        }

        public void SetToken(
            ApiController controller,
            HttpResponseHeaders headers,
            TokenDto tokenDto)
        {
            var cookie = new CookieHeaderValue(TOKEN_NAME, tokenDto.Token);
            cookie.Expires = tokenDto.Expiration;
            cookie.Domain = controller.Request.RequestUri.Host;
            cookie.Path = "/";
            headers.AddCookies(new[] { cookie });
        }

        public void Delete(ApiController controller, HttpResponseHeaders headers)
        {
            var cookie = new CookieHeaderValue(TOKEN_NAME, string.Empty);
            cookie.Expires = _utcTimeProvider.UtcNow.AddDays(-1);
            cookie.Domain = controller.Request.RequestUri.Host;
            cookie.Path = "/";
            headers.AddCookies(new[] { cookie });
        }

        public bool TryGet(ApiController controller, out string token)
        {
            var tokenCookie =
                controller.
                Request.
                Headers.
                GetCookies(TOKEN_NAME).
                OrderByDescending(c => c.Expires).
                FirstOrDefault();

            if (tokenCookie == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "Token cookie not found"
                });
            }

            token = tokenCookie[TOKEN_NAME].Value;
            return !string.IsNullOrEmpty(token);
        }
    }
}
