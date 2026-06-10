using TollFeeCalculator.Policies;

namespace TollFeeCalculator.Tests;

public class HolidayCalendarTests
{
    private readonly SwedishHolidayCalendar _calendar = new();

    [Theory]
    [InlineData(2013, 3, 18)]
    [InlineData(2013, 7, 15)]
    [InlineData(2024, 3, 12)]
    public void IsTollFreeDate_ReturnsFalse_OnRegularWeekday(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 8, 0, 0);

        Assert.False(_calendar.IsTollFreeDate(date));
    }

    [Theory]
    [InlineData(2013, 3, 16)]
    [InlineData(2013, 3, 17)]
    [InlineData(2024, 6, 8)]
    public void IsTollFreeDate_ReturnsTrue_OnWeekend(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }

    [Theory]
    [InlineData(2013, 1, 1)]
    [InlineData(2013, 3, 29)]
    [InlineData(2013, 6, 6)]
    [InlineData(2013, 12, 25)]
    public void IsTollFreeDate_ReturnsTrue_OnKnown2013Holiday(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }

    [Fact]
    public void IsTollFreeDate_ReturnsFalse_ForEntireJuly2013()
    {
        var date = new DateTime(2013, 7, 3, 8, 0, 0);

        Assert.False(_calendar.IsTollFreeDate(date));
    }

    [Fact]
    public void IsTollFreeDate_ReturnsTrue_ForCalculated2024Holiday()
    {
        var date = new DateTime(2024, 12, 25, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }
}
