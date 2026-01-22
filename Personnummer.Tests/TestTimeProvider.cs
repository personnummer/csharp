using System;

#if NET8_0_OR_GREATER
using TimeProvider = System.TimeProvider;
#else
using TimeProvider = Personnummer.TimeProvider;
#endif

namespace Personnummer.Tests;

/// <summary>
/// TimeProvider which always returns the same date: 2025 01 01 00:00:01 with UTC timezone on local time.
/// </summary>
public class TestTimeProvider : TimeProvider
{
    internal DateTimeOffset Now { get; set; } = new(
        new DateOnly(2025, 1, 1),
        new TimeOnly(0, 0, 0, 1),
        TimeSpan.Zero
    );

    public override DateTimeOffset GetUtcNow()
        => Now;
}
