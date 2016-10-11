using System.Net.Http.Headers;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public interface ITokenManager
    {
        /// <summary>
        /// Устанавливает токен для текущего пользователя
        /// </summary>
        void SetToken(ApiController controller, HttpResponseHeaders headers, TokenDto tokenDto);

        /// <summary>
        /// Возвращает токен текущего пользователя
        /// </summary>
        bool TryGet(ApiController controller, out string token);

        /// <summary>
        /// Удаляет токен текущего пользователя
        /// </summary>
        void Delete(ApiController controller, HttpResponseHeaders headers);
    }
}
