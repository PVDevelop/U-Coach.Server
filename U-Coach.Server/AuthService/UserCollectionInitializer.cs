using System;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService
{
    /// <summary>
    /// Создает коллекцию пользователей и навешивает нужные ключи, индексы
    /// </summary>
    public class UserCollectionInitializer : IMongoInitializer
    {
        private readonly IMongoConnectionSettings _metaSettings;

        public UserCollectionInitializer(IMongoConnectionSettings metaSettings)
        {
            if(metaSettings == null)
            {
                throw new ArgumentNullException("metaSettings");
            }
            _metaSettings = metaSettings;
        }

        public void Initialize()
        {
        }
    }
}
