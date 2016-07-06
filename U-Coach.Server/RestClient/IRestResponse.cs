namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestResponse
    {
        /// <summary>
        /// Возвращает содержимое ответа.
        /// Кидает<exception cref="RestExecutionException">если ответ содержит ошибку</exception>
        /// </summary>
        /// <returns></returns>
        string GetContentOrThrow();
    }
}
