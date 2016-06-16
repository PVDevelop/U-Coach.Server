using System;
using System.Linq.Expressions;

namespace PVDevelop.UCoach.Server.Mongo
{
    /// <summary>
    /// Продоставляет доступ к БД MongoDB
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMongoRepository<T>
        where T : IHaveId
    {
        /// <summary>
        /// Вставляет документ в коллекцию
        /// </summary>
        void Insert(string collection, T document);

        /// <summary>
        /// Находит единственный объект по предикату
        /// </summary>
        T Find(string collection, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Замещает имеющийся документ новым
        /// </summary>
        void Replace(string collection, T document);
    }
}
