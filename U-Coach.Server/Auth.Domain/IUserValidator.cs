namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public interface IUserValidator
    {
        /// <summary>
        /// Валидация логина
        /// </summary>
        void ValidateLogin(string login);

        /// <summary>
        /// Валидация пароля
        /// </summary>
        void ValidatePassword(string password);
    }
}
