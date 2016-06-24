using System;
using System.Linq.Expressions;

namespace PVDevelop.UCoach.Server.Mongo
{
    /// <summary>
    /// Продоставляет доступ к БД MongoDB
    /// </summary>
    public interface IMongoRepository<T>
    {
        /// <summary>
        /// Вставляет документ в коллекцию
        /// </summary>
        void Insert(T document);

        /// <summary>
        /// Находит единственный объект по предикату
        /// </summary>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Замещает имеющийся документ новым
        /// </summary>
        void ReplaceOne(Expression<Func<T, bool>> predicate, T document);
    }
}
