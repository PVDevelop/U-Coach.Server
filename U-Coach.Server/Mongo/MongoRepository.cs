using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MongoRepository<T> : IMongoRepository<T>
        where T : IHaveId
    {
        private readonly IMongoConnectionSettings _settings;

        public MongoRepository(
            IMongoConnectionSettings settings, 
            IMongoCollectionVersionValidator versionValidator)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            _settings = settings;
            versionValidator.Validate<T>();
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

        public void Replace(T document)
        {
            var coll = GetCollection();
            coll.ReplaceOne<T>(t => t.Id == document.Id, document);
        }

        private IMongoCollection<T> GetCollection()
        {
            return MongoHelper.GetCollection<T>(_settings);
        }
    }
}
