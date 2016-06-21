using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using PVDevelop.UCoach.Server.Exceptions.Mongo;

namespace PVDevelop.UCoach.Server.Mongo
{
    public class MongoCollectionVersionValidatorByClassAttribute :
        IMongoCollectionVersionValidator
    {
        private readonly IMongoConnectionSettings _settings;

        public MongoCollectionVersionValidatorByClassAttribute(IMongoConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            _settings = settings;
        }

        public void Validate<T>()
        {
            var collection = MongoHelper.GetCollection<CollectionVersion>(_settings);
            var referencedCollectionName = MongoHelper.GetCollectionName<T>();
            var collectionVersion = collection.Find(cv => cv.Name == referencedCollectionName).SingleOrDefault();

            var requiredVersion = MongoHelper.GetDataVersion<T>();
            if (collectionVersion == null)
            {
                throw new MongoCollectionNotInitializedException(0, requiredVersion);
            }
            if (collectionVersion.Version != requiredVersion)
            {
                throw new MongoCollectionNotInitializedException(collectionVersion.Version, requiredVersion);
            }
        }
    }
}
