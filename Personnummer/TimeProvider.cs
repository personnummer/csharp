using System;

namespace Personnummer;

public abstract class TimeProvider
{
    public static TimeProvider System { get; }
        = new DefaultTimeProvider();

    public abstract DateTimeOffset GetLocalNow();

    private sealed class DefaultTimeProvider : TimeProvider
    {
        public override DateTimeOffset GetLocalNow()
            => DateTimeOffset.Now;
    }
}