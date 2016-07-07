using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace TestWebApiUtilities
{
    internal class TestAssembliesResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            var files =
                Directory.
                GetFiles(directory).
                Where(fn => Path.GetFileNameWithoutExtension(fn).StartsWith("PVDevelop.UCoach.Server.Auth", System.StringComparison.InvariantCultureIgnoreCase)).
                Where(fn => Path.GetExtension(fn).ToLower() == ".dll").
                ToArray();

            return files.Select(fn => Assembly.LoadFrom(fn)).ToArray();
        }
    }
}