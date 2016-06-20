using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
