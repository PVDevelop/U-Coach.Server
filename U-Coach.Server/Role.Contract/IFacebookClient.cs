{
    public interface IFacebookClient
    {
        /// <summary>
        /// Возвращает url к странице авторизации
        /// </summary>
        /// <returns></returns>
        string GetAuthorizationUrl();
    }
}
