using System;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mapper;

namespace PVDevelop.UCoach.Server.Core.Mongo
{
    public class MongoSportsmanConfirmationRepository :
        ISportsmanConfirmationRepository
    {
        private readonly IMongoRepository<MongoSportsmanConfirmation> _repository;
        private readonly IMongoCollectionVersionValidator _collectionVersionValidator;

        public MongoSportsmanConfirmationRepository(
            IMongoCollectionVersionValidator collectionVersionValidator,
            IMongoRepository<MongoSportsmanConfirmation> repository)
        {
            if (collectionVersionValidator == null)
            {
                throw new ArgumentNullException(nameof(collectionVersionValidator));
            }
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _collectionVersionValidator = collectionVersionValidator;
            _repository = repository;
        }

        public void Insert(SportsmanConfirmation confirmation)
        { 
            _collectionVersionValidator.Validate<MongoSportsmanConfirmation>();
            var mongoConfirmation = MapperHelper.Map<SportsmanConfirmation, MongoSportsmanConfirmation>(confirmation);
            _repository.Insert(mongoConfirmation);
        }

        public SportsmanConfirmation FindByConfirmationKey(string key)
        {
            var mongoConfirmation = _repository.Find(sc => sc.ConfirmationKey == key);
            return MapperHelper.Map<MongoSportsmanConfirmation, SportsmanConfirmation>(mongoConfirmation);
        }
    }
}
