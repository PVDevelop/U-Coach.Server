using PVDevelop.UCoach.Server.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Tests
{
    public sealed class TestUtcTimeProvider : IUtcTimeProvider
    {
        public DateTime utcNow = DateTime.UtcNow;
        public DateTime UtcNow
        {
            get
            {
                return utcNow;
            }
        }
    }
}
