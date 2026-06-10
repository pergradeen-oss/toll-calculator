namespace TollFeeCalculator.Policies;

public sealed class TollFeePolicy : ITollFeePolicy
{
    private readonly (TimeSpan From, TimeSpan ToExclusive, int Fee)[] _tariff =
    [
        (new TimeSpan(6, 0, 0), new TimeSpan(6, 30, 0), 8),
        (new TimeSpan(6, 30, 0), new TimeSpan(7, 0, 0), 13),
        (new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0), 18),
        (new TimeSpan(8, 0, 0), new TimeSpan(8, 30, 0), 13),
        (new TimeSpan(8, 30, 0), new TimeSpan(15, 0, 0), 8),
        (new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0), 13),
        (new TimeSpan(15, 30, 0), new TimeSpan(17, 0, 0), 18),
        (new TimeSpan(17, 0, 0), new TimeSpan(18, 0, 0), 13),
        (new TimeSpan(18, 0, 0), new TimeSpan(18, 30, 0), 8),
    ];

    public int GetFee(DateTime date)
    {
        var time = TruncateToWholeSeconds(date.TimeOfDay);

        foreach (var (from, toExclusive, fee) in _tariff)
        {
            if (time >= from && time < toExclusive)
                return fee;
        }

        return 0;
    }

    private static TimeSpan TruncateToWholeSeconds(TimeSpan time) =>
        TimeSpan.FromSeconds((long)time.TotalSeconds);
}
