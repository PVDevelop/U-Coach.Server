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
    public sealed class MongoConfirmationRepository :
        IConfirmationRepository
    {
        private readonly IMongoRepository<MongoConfirmation> _repository;

        public MongoConfirmationRepository(
            IMongoRepository<MongoConfirmation> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public Confirmation FindByConfirmation(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var mongoConfirmation = _repository.Find(u => u.Key == key);
            return MapperHelper.Map<MongoConfirmation, Confirmation>(mongoConfirmation);
        }

        public Confirmation FindByConfirmationByUserId(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            _versionCollectionValidator.Validate<MongoConfirmation>();

            var mongoConfirmation = _repository.Find(u => u.UserId == userId);
            return MapperHelper.Map<MongoConfirmation, Confirmation>(mongoConfirmation);
        }

        public void Replace(Confirmation confirmation)
        {
            if (confirmation == null)
            {
                throw new ArgumentNullException(nameof(confirmation));
            }
            var mongoConfirmation = MapperHelper.Map<Confirmation, MongoConfirmation>(confirmation);

            _repository.ReplaceOne(u => u.UserId == mongoConfirmation.UserId, mongoConfirmation);
        }

        public void Delete(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            _repository.Remove(u => u.Key == key);
        }
    }
}
