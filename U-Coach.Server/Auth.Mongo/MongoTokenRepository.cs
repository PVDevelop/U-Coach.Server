using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mapper;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    public class MongoTokenRepository :
        ITokenRepository
    {
        private readonly IMongoRepository<MongoToken> _repository;

        public MongoTokenRepository(
            IMongoRepository<MongoToken> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public void AddToken(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var mongotoken = MapperHelper.Map<Token, MongoToken>(token);
            _repository.Insert(mongotoken);
        }

        public Token GetToken(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var mongotoken = _repository.Find((u) => u.Key == token);
            return MapperHelper.Map<MongoToken, Token>(mongotoken);
        }
    }
}
