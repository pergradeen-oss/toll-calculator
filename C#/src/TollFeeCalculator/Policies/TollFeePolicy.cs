namespace TollFeeCalculator.Policies;

public sealed class TollFeePolicy : ITollFeePolicy
{
    private readonly (TimeSpan From, TimeSpan To, int Fee)[] _tariff =
    [
        (new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), 8),
        (new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), 13),
        (new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), 18),
        (new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 59), 13),
        (new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 59), 8),
        (new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59), 13),
        (new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 59), 18),
        (new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 59), 13),
        (new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 59), 8),
    ];

    public int GetFee(DateTime date)
    {
        var time = date.TimeOfDay;

        foreach (var (from, to, fee) in _tariff)
        {
            if (time >= from && time <= to)
                return fee;
        }

        return 0;
    }
}
