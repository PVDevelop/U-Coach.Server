using System;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Mongo
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IMongoRepository<MongoToken> _repository;

        public TokenRepository(
            IMongoRepository<MongoToken> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public void Insert(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var mongoToken = MapToMongoToken(token);

            _repository.Insert(mongoToken);
        }

        public bool TryGet(TokenId id, out Token token)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            MongoToken mongoToken;
            if (_repository.TryFind(t => !t.IsDeleted && t.Id.Equals(id), out mongoToken))
            {
                token = Mapper.MapperHelper.Map<MongoToken, Token>(mongoToken);
                return true;
            }

            token = null;
            return false;
        }

        public void Update(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var mongoToken = MapToMongoToken(token);
            _repository.ReplaceOne(t => t.Id.Equals(mongoToken.Id), mongoToken);
        }

        private MongoToken MapToMongoToken(Token token)
        {
            return Mapper.MapperHelper.Map<Token, MongoToken>(token);
        }
    }
}
