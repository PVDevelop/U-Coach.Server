using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    /// <summary>
    /// Инициализирует БД
    /// </summary>
    public interface IMongoInitializer
    {
        void Initialize();
    }
}
