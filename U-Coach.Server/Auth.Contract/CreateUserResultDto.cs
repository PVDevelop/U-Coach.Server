namespace PVDevelop.UCoach.Server.Auth.Contract
{
    /// <summary>
    /// Результат создания нового пользователя
    /// </summary>
    public class CreateUserResultDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }
    }
}
