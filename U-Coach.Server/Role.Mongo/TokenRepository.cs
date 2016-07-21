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

            var mongoToken = new MongoToken()
            {
                Id = token.Id,
                UserId = token.UserId,
                AuthToken = token.AuthToken,
                Expiration = token.Expiration
            };

            _repository.Insert(mongoToken);
        }

        public bool TryGet(TokenId id, out Token token)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            MongoToken mongoToken;
            if (_repository.TryFind(t => t.Id.Equals(id), out mongoToken))
            {
                token = new Token(mongoToken.Id, mongoToken.UserId, mongoToken.AuthToken, mongoToken.Expiration);
                return true;
            }

            token = null;
            return false;
        }
    }
}
