using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MongoRepository<T> : IMongoRepository<T>
        where T : IHaveId
    {
        private readonly IMongoConnectionSettings _settings;

        public MongoRepository(IMongoConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            _settings = settings;
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
            var builder = new MongoUrlBuilder(_settings.ConnectionString);

            var mongoClient = new MongoClient(builder.ToMongoUrl());
            var db = mongoClient.GetDatabase(builder.DatabaseName);

            var collectionName = MongoHelper.GetCollectionName<T>();

            return db.GetCollection<T>(collectionName);
        }
    }
}
