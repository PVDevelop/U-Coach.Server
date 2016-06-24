using System;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoRepository<MongoUser> _repository;
        private readonly IMapper _mapper;

        public MongoUserRepository(
            IMongoRepository<MongoUser> repository,
            IMapper mapper)
        {
            if(repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if(mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            _repository = repository;
            _mapper = mapper;
        }

        public void Insert(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }
            var mongoUser = _mapper.Map<MongoUser>(user);
            _repository.Insert(mongoUser);
        }

        public User FindByLogin(string login)
        {
            var mongoUser = _repository.Find(u => u.Login == login);
            return _mapper.Map<User>(mongoUser);
        }

        public void Update(User user)
        {
            var mongoUser = _mapper.Map<MongoUser>(user);
            _repository.Replace(mongoUser);
        }
    }
}
