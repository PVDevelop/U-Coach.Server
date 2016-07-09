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
        /// Проверяет результат выполнения GET запроса на наличие ошибок
        /// </summary>
        IRestResponse CheckGetResult();

        /// <summary>
        /// Проверяет результат выполнение POST запроса на наличие ошибок
        /// </summary>
        IRestResponse CheckPostResult();

        /// <summary>
        /// Проверяет результат выполнения PUT запроса на наличие ошибок
        /// </summary>
        IRestResponse CheckPutResult();
    }
}
