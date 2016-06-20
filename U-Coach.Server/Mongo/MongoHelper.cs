using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    public static class MongoHelper
    {
        public static string GetCollectionName<T>()
        {
            return nameof(T);
        }
    }
}
