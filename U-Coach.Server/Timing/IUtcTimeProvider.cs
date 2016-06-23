using System;

namespace Timing
{
    public interface IUtcTimeProvider
    {
        DateTime UtcTime { get; }
    }
}
