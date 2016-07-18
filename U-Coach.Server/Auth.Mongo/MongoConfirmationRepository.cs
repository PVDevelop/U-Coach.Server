﻿using PVDevelop.UCoach.Server.Auth.Service;
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
        private readonly IMongoCollectionVersionValidator _versionCollectionValidator;


        public MongoConfirmationRepository(
            IMongoRepository<MongoConfirmation> repository,
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

        public Confirmation FindByConfirmation(string userId, string key)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _versionCollectionValidator.Validate<MongoConfirmation>();

            var mongoConfirmation = _repository.Find(u => u.UserId == userId && u.ConfirmationKey == key);
            return MapperHelper.Map<MongoConfirmation, Confirmation>(mongoConfirmation);
        }

        public void Obtain(Confirmation confirmation)
        {
            if (confirmation == null)
            {
                throw new ArgumentNullException(nameof(confirmation));
            }
            _versionCollectionValidator.Validate<MongoConfirmation>();
            var mongoConfirmation = MapperHelper.Map<Confirmation, MongoConfirmation>(confirmation);

            _repository.ReplaceOne(u => u.UserId == mongoConfirmation.UserId, mongoConfirmation);
        }
    }
}
