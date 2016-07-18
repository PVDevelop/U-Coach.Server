using PVDevelop.UCoach.Server.Auth.Service;
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
        private readonly IMongoCollectionVersionValidator _versionCollectionValidator;

        public MongoTokenRepository(
            IMongoRepository<MongoToken> repository,
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

        public void AddToken(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            _versionCollectionValidator.Validate<MongoToken>();

            var mongotoken = MapperHelper.Map<Token, MongoToken>(token);
            _repository.Insert(mongotoken);
        }

        public void CloseToken(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            _versionCollectionValidator.Validate<MongoToken>();

            var mongoToken = _repository.Find(it => it.Key == token);
            _repository.TryRemove(mongoToken);
        }

        public Token GetToken(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            _versionCollectionValidator.Validate<MongoToken>();

            var mongotoken = _repository.Find((u) => u.Key == token);
            return MapperHelper.Map<MongoToken, Token>(mongotoken);
        }
    }
}
