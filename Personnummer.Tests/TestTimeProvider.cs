using System;

namespace Personnummer.Tests;

/// <summary>
/// TimeProvider which always returns the same date: 2025 01 01 00:00:01 with UTC timezone on local time.
/// </summary>
public class TestTimeProvider : TimeProvider
{
    public override DateTimeOffset GetUtcNow()
    {
        return new DateTimeOffset(
            new DateOnly(2025, 1, 1),
            new TimeOnly(0,0,0, 1),
            TimeSpan.Zero
        );
    }

    public override TimeZoneInfo LocalTimeZone { get; } = TimeZoneInfo.Utc;
}
