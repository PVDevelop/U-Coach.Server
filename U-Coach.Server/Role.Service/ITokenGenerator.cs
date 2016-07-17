using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    /// <summary>
    /// Интерфейс генерации токена пользователя на основе токена внешней системы
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// Сгенерировать токен
        /// </summary>
        string Generate(User user, AuthTokenParams tokenParams);
    }
}