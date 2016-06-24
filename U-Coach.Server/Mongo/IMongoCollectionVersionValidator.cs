namespace PVDevelop.UCoach.Server.Mongo
{
    public interface IMongoCollectionVersionValidator
    {
        /// <summary>
        /// Проверяет версию данных T и в случае, если версия невалидна, кидает InvalidDataVersionException
        /// </summary>
        void Validate<T>();
    }
}
