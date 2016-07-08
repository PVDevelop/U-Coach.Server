using System;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Role.Service;

namespace PVDevelop.UCoach.Server.Role.Mongo
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<MongoUser> _repository;
        private readonly IUserFactory _factory;

        public UserRepository(
            IUserFactory factory,
            IMongoRepository<MongoUser> repository)
        {
            if(factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _factory = factory;
            _repository = repository;
        }

        public bool Contains(UserId id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return 
                _repository
                .Contains(u => u.Id.AuthId == id.AuthId && u.Id.AuthSystemName == id.AuthSystemName);
        }

        public void Insert(IUser user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var mongoUser = MapperHelper.Map<IUser, MongoUser>(user);
            _repository.Insert(mongoUser);
        }
    }
}
