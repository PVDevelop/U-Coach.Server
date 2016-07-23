using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using PVDevelop.UCoach.Server.Configuration;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MongoRepository<T> : IMongoRepository<T>
    {
        private readonly IConnectionStringProvider _settings;

        public MongoRepository(
            IConnectionStringProvider settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _settings = settings;
        }

        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection();
            return coll.Find(predicate).Any();
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, out T item)
        {
            var coll = GetCollection();
            item = coll.Find(predicate).SingleOrDefault();
            return item != null;
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

        public void Remove(Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection();
            coll.DeleteMany(predicate);
        }

        private IMongoCollection<T> GetCollection()
        {
            return MongoHelper.GetCollection<T>(_settings);
        }
    }
}
