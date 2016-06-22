using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MongoIndexNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public MongoIndexNameAttribute(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
        }
    }
}
