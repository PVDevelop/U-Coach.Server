using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class MongoCollectionAttribute : Attribute
    {
        public string Name { get; private set; }

        public MongoCollectionAttribute(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
        }
    }
}
