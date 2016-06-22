using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    [MongoCollection("Versions")]
    public class CollectionVersion
    {
        public int Id { get; set; }
        [MongoIndexName("name")]
        public string Name { get; set; }
        public int Version { get; set; }
    }
}
