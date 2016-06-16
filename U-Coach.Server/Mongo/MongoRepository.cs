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

        public T Find(string collection, Expression<Func<T, bool>> predicate)
        {
            var coll = GetCollection(collection);
            return coll.Find(predicate).Single();
        }

        public void Insert(string collection, T document)
        {
            var coll = GetCollection(collection);
            coll.InsertOne(document);
        }

        public void Replace(string collection, T document)
        {
            var coll = GetCollection(collection);
            coll.ReplaceOne<T>(t => t.Id == document.Id, document);
        }

        private IMongoCollection<T> GetCollection(string collection)
        {
            var builder = new MongoUrlBuilder(_settings.ConnectionString);

            var mongoClient = new MongoClient(builder.ToMongoUrl());
            var db = mongoClient.GetDatabase(builder.DatabaseName);

            return db.GetCollection<T>(collection);
        }
    }
}
