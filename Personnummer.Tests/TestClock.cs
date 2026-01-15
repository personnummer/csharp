using System;

namespace Personnummer.Tests;

public sealed class TestClock
{
    public DateTimeOffset Now { get; set; } = new(
        new DateTime(2025, 1, 1, 0, 0, 1),
        TimeSpan.Zero
    );

    public DateTimeOffset GetNow()
    {
        return Now;
    }
}