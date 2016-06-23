using System;
using Timing;

namespace PVDevelop.UCoach.Server.Timing
{
    public class SystemUtcTimeProvider : IUtcTimeProvider
    {
        public DateTime UtcTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
