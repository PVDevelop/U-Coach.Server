using System;
using PVDevelop.UCoach.Server.Timing;

namespace TestTimingUtilities
{
    public class FixedUtcTimeProvider : IUtcTimeProvider
    {
        public FixedUtcTimeProvider()
        {
            UtcNow = DateTime.UtcNow;
        }

        public DateTime UtcNow { get; private set; }
    }
}
