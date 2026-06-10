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
    public void IsTollFreeDate_ReturnsTrue_OnCalculatedHoliday(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }

    [Fact]
    public void IsTollFreeDate_ReturnsFalse_ForRegularWeekdayInJuly2013()
    {
        var date = new DateTime(2013, 7, 3, 8, 0, 0);

        Assert.False(_calendar.IsTollFreeDate(date));
    }

    [Fact]
    public void IsTollFreeDate_ReturnsTrue_ForCalculatedMidsummer2013()
    {
        var date = new DateTime(2013, 6, 22, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }

    [Theory]
    [InlineData(2013, 4, 30)]
    [InlineData(2013, 6, 21)]
    [InlineData(2013, 11, 1)]
    public void IsTollFreeDate_ReturnsFalse_ForNonHolidayDatesIn2013(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 8, 0, 0);

        Assert.False(_calendar.IsTollFreeDate(date));
    }

    [Fact]
    public void IsTollFreeDate_ReturnsTrue_ForCalculated2024Holiday()
    {
        var date = new DateTime(2024, 12, 25, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }

    [Theory]
    [InlineData(2013, 3, 28)]  // Skärtorsdag  (påskdagen 31 mars)
    [InlineData(2013, 3, 29)]  // Långfredag
    [InlineData(2013, 4, 1)]   // Annandag påsk
    [InlineData(2013, 5, 9)]   // Kristi himmelsfärds dag
    [InlineData(2013, 5, 19)]  // Pingstdagen
    [InlineData(2022, 4, 14)]  // Skärtorsdag  (påskdagen 17 april)
    [InlineData(2022, 4, 15)]  // Långfredag
    [InlineData(2022, 4, 18)]  // Annandag påsk
    [InlineData(2022, 5, 26)]  // Kristi himmelsfärds dag
    [InlineData(2022, 6, 5)]   // Pingstdagen
    [InlineData(2025, 4, 17)]  // Skärtorsdag  (påskdagen 20 april)
    [InlineData(2025, 4, 18)]  // Långfredag
    [InlineData(2025, 4, 21)]  // Annandag påsk
    [InlineData(2025, 5, 29)]  // Kristi himmelsfärds dag
    [InlineData(2025, 6, 8)]   // Pingstdagen
    public void IsTollFreeDate_ReturnsTrue_ForEasterDerivedHolidays(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }

    [Theory]
    [InlineData(2013, 3, 31)]  // Påskdagen
    [InlineData(2022, 4, 17)]
    [InlineData(2025, 4, 20)]
    public void IsTollFreeDate_ReturnsTrue_OnEasterSunday(int year, int month, int day)
    {
        var date = new DateTime(year, month, day, 8, 0, 0);

        Assert.True(_calendar.IsTollFreeDate(date));
    }
}
