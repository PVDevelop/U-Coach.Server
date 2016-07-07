namespace PVDevelop.UCoach.Server.RestClient
{
    public interface IRestResponse
    {
        /// <summary>
        /// Возвращает содержимое ответа.
        /// </summary>
        T GetContent<T>() where T : class;

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
