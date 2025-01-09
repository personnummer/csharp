using System;

namespace Personnummer.Tests;

#if NET8_0_OR_GREATER

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
    {
        return Now;
    }

    public override TimeZoneInfo LocalTimeZone { get; } = TimeZoneInfo.Utc;
}

#endif
