using TollFeeCalculator.Policies;

namespace TollFeeCalculator.Tests;

public class TollFeePolicyTests
{
    private readonly TollFeePolicy _policy = new();
    private static readonly DateOnly BaseDate = new(2013, 3, 18);

    [Theory]
    [InlineData(6, 0, 8)]
    [InlineData(6, 29, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(6, 59, 13)]
    [InlineData(7, 0, 18)]
    [InlineData(7, 59, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(8, 29, 13)]
    [InlineData(8, 30, 8)]
    [InlineData(9, 0, 8)]
    [InlineData(9, 29, 8)]
    [InlineData(10, 15, 8)]
    [InlineData(14, 29, 8)]
    [InlineData(14, 59, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 29, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(16, 59, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(17, 59, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 29, 8)]
    public void GetFee_ReturnsExpectedFee_ForTariffZones(int hour, int minute, int expectedFee)
    {
        var date = BaseDate.ToDateTime(new TimeOnly(hour, minute));

        Assert.Equal(expectedFee, _policy.GetFee(date));
    }

    [Theory]
    [InlineData(5, 59)]
    [InlineData(18, 30)]
    [InlineData(19, 0)]
    [InlineData(0, 0)]
    public void GetFee_ReturnsZero_OutsideTariffHours(int hour, int minute)
    {
        var date = BaseDate.ToDateTime(new TimeOnly(hour, minute));

        Assert.Equal(0, _policy.GetFee(date));
    }
}
