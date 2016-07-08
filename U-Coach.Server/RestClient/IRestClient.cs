namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestClient
    {
        /// <summary>
        /// Добавляет тело в сообщение
        /// </summary>
        IRestClient AddBody(object body);

        /// <summary>
        /// Добавляет параметр в запрос
        /// </summary>
        IRestClient AddParameter(string name, string value);

        /// <summary>
        /// Выполняет синхронный запрос и возвращает результат
        /// </summary>
        IRestResponse Execute();
    }
}
