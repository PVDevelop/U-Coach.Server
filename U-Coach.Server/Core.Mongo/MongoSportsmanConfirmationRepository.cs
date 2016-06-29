using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.Core.Mongo
{
    public class MongoSportsmanConfirmationRepository :
        ISportsmanConfirmationRepository
    {
        private readonly IMongoRepository<MongoSportsmanConfirmation> _repository;
        private readonly IMongoCollectionVersionValidator _collectionVersionValidator;
        private readonly IMapper _mapper;

        public MongoSportsmanConfirmationRepository(
            IMongoCollectionVersionValidator collectionVersionValidator,
            IMongoRepository<MongoSportsmanConfirmation> repository,
            IMapper mapper)
        {
            if (collectionVersionValidator == null)
            {
                throw new ArgumentNullException("collectionVersionValidator");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            _collectionVersionValidator = collectionVersionValidator;
            _repository = repository;
            _mapper = mapper;
        }

        public void Insert(SportsmanConfirmation confirmation)
        {
            _collectionVersionValidator.Validate<MongoSportsmanConfirmation>();
            var mongoConfirmation = _mapper.Map<MongoSportsmanConfirmation>(confirmation);
            _repository.Insert(mongoConfirmation);
        }

        public SportsmanConfirmation FindByConfirmationKey(string key)
        {
            var mongoConfirmation = _repository.Find(sc => sc.ConfirmationKey == key);
            return _mapper.Map<SportsmanConfirmation>(mongoConfirmation);
        }
    }
}
