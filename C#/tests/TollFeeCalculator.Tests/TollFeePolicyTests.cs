using TollFeeCalculator.Policies;

namespace TollFeeCalculator.Tests;

public class TollFeePolicyTests
{
    private readonly TollFeePolicy _policy = new();
    private static readonly DateOnly BaseDate = new(2013, 3, 18);

    [Theory]
    // 06:00 - 06:29     8 kr
    [InlineData(8, 6, 0)]
    [InlineData(8, 6, 29)]
    // 06:30 - 06:59     13 kr
    [InlineData(13, 6, 30)]
    [InlineData(13, 6, 59)]
    // 07:00 - 07:59     18 kr
    [InlineData(18, 7, 0)]
    [InlineData(18, 7, 59)]
    // 08:00 - 08:29     13 kr
    [InlineData(13, 8, 0)]
    [InlineData(13, 8, 29)]
    // 08:30 - 14:59     8 kr
    [InlineData(8, 8, 30)]
    [InlineData(8, 9, 0)]
    [InlineData(8, 9, 29)]
    [InlineData(8, 10, 15)]
    [InlineData(8, 14, 29)]
    [InlineData(8, 14, 59)]
    // 15:00 - 15:29     13 kr
    [InlineData(13, 15, 0)]
    [InlineData(13, 15, 29)]
    // 15:30 - 16:59     18 kr
    [InlineData(18, 15, 30)]
    [InlineData(18, 16, 59)]
    // 17:00 - 17:59     13 kr
    [InlineData(13, 17, 0)]
    [InlineData(13, 17, 59)]
    // 18:00 - 18:29     8 kr
    [InlineData(8, 18, 0)]
    [InlineData(8, 18, 29)]
    // 18:30 - 05:59     0 kr
    [InlineData(0, 18, 30)]
    [InlineData(0, 5, 59)]
    [InlineData(0, 0, 0)]
    [InlineData(0, 19, 0)]
    public void GetFee_ReturnsExpectedFee_ForAllTariffZones(int expectedFee, int hour, int minute)
    {
        var date = BaseDate.ToDateTime(new TimeOnly(hour, minute));

        Assert.Equal(expectedFee, _policy.GetFee(date));
    }

    [Fact]
    public void GetFee_TruncatesMilliseconds_ToWholeSeconds()
    {
        var date = new DateTime(2013, 3, 18, 6, 29, 59, 750);

        Assert.Equal(8, _policy.GetFee(date));
    }

    [Fact]
    public void GetFee_TruncatesMilliseconds_BeforeApplyingNextTariffSlot()
    {
        var date = new DateTime(2013, 3, 18, 6, 30, 0, 250);

        Assert.Equal(13, _policy.GetFee(date));
    }

    [Fact]
    public void GetFee_ReturnsZero_AtHalfOpenIntervalBoundary()
    {
        var date = BaseDate.ToDateTime(new TimeOnly(18, 30));

        Assert.Equal(0, _policy.GetFee(date));
    }
}
