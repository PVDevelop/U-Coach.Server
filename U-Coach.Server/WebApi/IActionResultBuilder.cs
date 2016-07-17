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
        /// Возвращает асинхронный http ответ
        /// </summary>
        Task<HttpResponseMessage> BuildAsync(string resource);
    }
}
