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
        private readonly IMapper _mapper;

        public MongoUserRepository(
            IMongoRepository<MongoUser> repository,
            IMongoCollectionVersionValidator versionCollectionValidator,
            IMapper mapper)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            if (versionCollectionValidator == null)
            {
                throw new ArgumentNullException("versionCollectionValidator");
            }

            _repository = repository;
            _mapper = mapper;
            _versionCollectionValidator = versionCollectionValidator;
        }

        public void Insert(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _versionCollectionValidator.Validate<MongoUser>();

            var mongoUser = _mapper.Map<MongoUser>(user);
            _repository.Insert(mongoUser);
        }

        public User FindByLogin(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var mongoUser = _repository.Find(u => u.Login == login);
            return _mapper.Map<User>(mongoUser);
        }

        public void Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _versionCollectionValidator.Validate<MongoUser>();

            var mongoUser = _mapper.Map<MongoUser>(user);
            _repository.ReplaceOne(u => u.Id == user.Id, mongoUser);
        }
    }
}
