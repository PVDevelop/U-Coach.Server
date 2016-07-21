using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.WebApi
{
    public interface IActionResultBuilder : IDisposable
    {
        /// <summary>
        /// Добавить параметр в Url
        /// </summary>
        IActionResultBuilder AddParameter(string key, string value);

        /// <summary>
        /// Возвращает асинхронный http-get ответ
        /// </summary>
        Task<HttpResponseMessage> BuildGetAsync(string resource);

        /// <summary>
        /// Возвращает асинхронный http-post ответ
        /// </summary>
        Task<HttpResponseMessage> BuildPostAsync(string resource, HttpContent content);
    }
}
