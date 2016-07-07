using System;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Service;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoRepository<MongoUser> _repository;
        private readonly IMongoCollectionVersionValidator _versionCollectionValidator;

        public MongoUserRepository(
            IMongoRepository<MongoUser> repository,
            IMongoCollectionVersionValidator versionCollectionValidator)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (versionCollectionValidator == null)
            {
                throw new ArgumentNullException(nameof(versionCollectionValidator));
            }

            _repository = repository;
            _versionCollectionValidator = versionCollectionValidator;
        }

        public void Insert(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _versionCollectionValidator.Validate<MongoUser>();

            var mongoUser = MapperHelper.Map<User, MongoUser>(user);
            _repository.Insert(mongoUser);
        }

        public User FindByLogin(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var mongoUser = _repository.Find(u => u.Login == login);
            return MapperHelper.Map<MongoUser, User>(mongoUser);
        }

        public void Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _versionCollectionValidator.Validate<MongoUser>();

            var mongoUser = MapperHelper.Map<User, MongoUser>(user);
            _repository.ReplaceOne(u => u.Id == user.Id, mongoUser);
        }
    }
}
