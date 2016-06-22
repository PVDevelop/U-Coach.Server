using System;
using System.Linq.Expressions;

namespace PVDevelop.UCoach.Server.Mongo
{
    /// <summary>
    /// Продоставляет доступ к БД MongoDB
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMongoRepository<T>
        where T : IAmDocument
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
        void Replace(T document);
    }
}
