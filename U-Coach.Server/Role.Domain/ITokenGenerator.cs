using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    /// <summary>
    /// Интерфейс генерации токена пользователя на основе токена внешней системы
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// Сгенерировать токен
        /// </summary>
        string Generate(User user, AuthSystemToken authToken);
    }
}