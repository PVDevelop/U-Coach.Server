namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestResponse
    {
        /// <summary>
        /// Возвращает содержимое ответа, либо кидает исключение.
        /// </summary>
        /// <exception cref="RestExecutionException"/>
        string GetContentOrThrow();

        /// <summary>
        /// Проверяет результат на наличие ошибок
        /// </summary>
        void CheckResult();
    }
}
