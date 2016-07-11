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
        private readonly IUserFactory _userFactory;

        public UserRepository(
            IMongoRepository<MongoUser> repository,
            IUserFactory userFactory)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if(userFactory == null)
            {
                throw new ArgumentNullException(nameof(userFactory));
            }

            _repository = repository;
            _userFactory = userFactory;
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

        public IUser Get(UserId id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var mongoUser = _repository.Find(u => u.Id.Equals(id));
            var user = _userFactory.CreateUser(mongoUser.Id);
            user.SetToken(mongoUser.Token);
            return user;
        }
    }
}
