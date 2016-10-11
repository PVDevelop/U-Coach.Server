using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class UtilitiesExtensions
    {
        public static void NullValidate(this object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void NullOrEmptyValidate(this string obj, string name)
        {
            if (String.IsNullOrEmpty(obj))
            {
                throw new ArgumentException($"Object with name {obj} is null or empty");
            }
        }
    }
}
