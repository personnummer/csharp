using System;

namespace Personnummer;

#if (!NET8_0_OR_GREATER)
public abstract class TimeProvider
{
    public virtual DateTimeOffset GetLocalNow() => GetUtcNow().ToLocalTime();

    public static TimeProvider System { get; }
        = new FallbackTimeProvider();

    public abstract DateTimeOffset GetUtcNow();

    private sealed class FallbackTimeProvider : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
            => DateTimeOffset.UtcNow;
    }
}
#endif