using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timing;

namespace TestUtils
{
    public class FixedUtcTimeProvider : IUtcTimeProvider
    {
        public DateTime UtcTime { get; set; }

        public FixedUtcTimeProvider()
        {
            SetUtcNow();
        }

        public void SetUtcNow()
        {
            UtcTime = DateTime.UtcNow;
        }
    }
}
