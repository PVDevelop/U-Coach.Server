using System;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Mongo
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<MongoUser> _repository;

        public UserRepository(
            IMongoRepository<MongoUser> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public void Insert(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var mongoUser = new MongoUser()
            {
                Id = user.Id
            };

            _repository.Insert(mongoUser);
        }

        public bool TryGet(UserId id, out User user)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            MongoUser mongoUser;
            if (_repository.TryFind(u => u.Id.Equals(id), out mongoUser))
            {
                user = new User(id);
                return true;
            }

            user = null;
            return false;
        }
    }
}
