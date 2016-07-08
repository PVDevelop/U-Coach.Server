using System;

namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestResponse
    {
        /// <summary>
        /// Возвращает содержимое ответа в изначальном виде
        /// </summary>
        /// <returns></returns>
        string GetContent();

        /// <summary>
        /// Десериализует содержимое ответа формата JSON в тип T
        /// </summary>
        T GetJsonContent<T>() where T : class;

        /// <summary>
        /// Проверяет результат на наличие ошибок
        /// </summary>
        /// <exception cref="RestExecutionException"/>
        IRestResponse CheckPostResult();

        /// <summary>
        /// Проверяет результат на наличие ошибок
        /// </summary>
        /// <exception cref="RestExecutionException"/>
        IRestResponse CheckPutResult();
    }
}
