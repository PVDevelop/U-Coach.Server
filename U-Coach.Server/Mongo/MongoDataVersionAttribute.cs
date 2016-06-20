using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MongoDataVersionAttribute : Attribute
    {
        public int Version { get; private set; }

        public MongoDataVersionAttribute(int version)
        {
            if(version <= 0)
            {
                throw new ArgumentOutOfRangeException("version");
            }

            Version = version;
        }
    }
}
