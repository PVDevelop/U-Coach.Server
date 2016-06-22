using System;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService
{
    /// <summary>
    /// Создает коллекцию пользователей и навешивает нужные ключи, индексы
    /// </summary>
    public class UserCollectionInitializer : IMongoInitializer
    {
        private readonly IMongoRepository<User> _repository;

        public UserCollectionInitializer(IMongoRepository<User> repository)
        {
            if(repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
