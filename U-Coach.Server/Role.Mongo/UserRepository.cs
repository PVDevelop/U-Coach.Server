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

        public void Insert(IUser user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var mongoUser = MapperHelper.Map<IUser, MongoUser>(user);
            _repository.Insert(mongoUser);
        }

        public void Update(IUser user)
        {
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }

            var mongoUser = new MongoUser()
            {
                Id = user.Id,
                Token = user.Token
            };

            _repository.ReplaceOne(u => u.Id.Equals(mongoUser.Id), mongoUser);
        }

        public bool TryGet(UserId id, out IUser user)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            MongoUser mongoUser;
            if (_repository.TryFind(u => u.Id.Equals(id), out mongoUser))
            {
                user = _userFactory.CreateUser(mongoUser.Id);
                user.SetToken(mongoUser.Token);
                return true;
            }

            user = null;
            return false;
        }
    }
}
