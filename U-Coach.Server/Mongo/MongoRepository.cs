using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MongoRepository<T> : IMongoRepository<T>
    {
        private readonly IMongoConnectionSettings _settings;
        private readonly IMongoCollectionVersionValidator _validator;

        public MongoRepository(
            IMongoConnectionSettings settings, 
            IMongoCollectionVersionValidator versionValidator)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (versionValidator == null)
            {
                throw new ArgumentNullException("versionValidator");
            }

            _settings = settings;
            _validator = versionValidator;
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection();
            return coll.Find(predicate).Single();
        }

        public void Insert(T document)
        {
            var coll = GetCollection();
            coll.InsertOne(document);
        }

        public void ReplaceOne(Expression<Func<T, bool>> predicate, T document)
        {
            var coll = GetCollection();
            coll.ReplaceOne<T>(predicate, document);
        }

        private IMongoCollection<T> GetCollection()
        {
            _validator.Validate<T>();
            return MongoHelper.GetCollection<T>(_settings);
        }
    }
}
