using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    private static readonly DateOnly Weekday = new(2013, 3, 18);
    private readonly TollCalculator _calculator = new();

    [Fact]
    public void GetTollFee_ReturnsZero_WhenDatesArrayIsEmpty()
    {
        var vehicle = new Car();

        Assert.Equal(0, _calculator.GetTollFee(vehicle));
    }

    [Fact]
    public void GetTollFee_Throws_WhenDatesIsNull()
    {
        var vehicle = new Car();

        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(vehicle, null!));
    }

    [Fact]
    public void GetTollFee_Throws_WhenVehicleIsNull()
    {
        var date = Weekday.ToDateTime(new TimeOnly(8, 0));

        Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(null!, date));
    }

    [Fact]
    public void GetTollFee_ReturnsZero_ForExemptVehicle()
    {
        var vehicle = new Motorbike();
        var dates = new[]
        {
            Weekday.ToDateTime(new TimeOnly(7, 0)),
            Weekday.ToDateTime(new TimeOnly(8, 0))
        };

        Assert.Equal(0, _calculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_ReturnsZero_OnHoliday()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 12, 25, 7, 0, 0);

        Assert.Equal(0, _calculator.GetTollFee(date, vehicle));
    }

    [Theory]
    [InlineData(6, 0, 8)]
    [InlineData(7, 0, 18)]
    [InlineData(9, 15, 8)]
    [InlineData(15, 45, 18)]
    public void GetTollFee_ReturnsSinglePassFee(int hour, int minute, int expectedFee)
    {
        var vehicle = new Car();
        var date = Weekday.ToDateTime(new TimeOnly(hour, minute));

        Assert.Equal(expectedFee, _calculator.GetTollFee(date, vehicle));
    }

    [Fact]
    public void GetTollFee_ChargesHighestFee_WithinSameHourWindow()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            Weekday.ToDateTime(new TimeOnly(7, 0)),
            Weekday.ToDateTime(new TimeOnly(7, 30))
        };

        Assert.Equal(18, _calculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_ChargesAgain_WhenMoreThanSixtyMinutesHavePassed()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            Weekday.ToDateTime(new TimeOnly(7, 0)),
            Weekday.ToDateTime(new TimeOnly(8, 30))
        };

        Assert.Equal(26, _calculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_DoesNotDoubleCharge_PassesWithinHourAfterNewInterval()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            Weekday.ToDateTime(new TimeOnly(7, 0)),
            Weekday.ToDateTime(new TimeOnly(8, 30)),
            Weekday.ToDateTime(new TimeOnly(8, 45))
        };

        Assert.Equal(26, _calculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_CapsDailyTotalAtSixtySek()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            Weekday.ToDateTime(new TimeOnly(7, 0)),
            Weekday.ToDateTime(new TimeOnly(8, 1)),
            Weekday.ToDateTime(new TimeOnly(9, 2)),
            Weekday.ToDateTime(new TimeOnly(10, 3)),
            Weekday.ToDateTime(new TimeOnly(11, 4)),
            Weekday.ToDateTime(new TimeOnly(12, 5))
        };

        Assert.Equal(60, _calculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_AppliesDailyCap_PerCalendarDay()
    {
        var vehicle = new Car();
        var dayOnePasses = new[]
        {
            new DateTime(2013, 3, 18, 7, 0, 0),
            new DateTime(2013, 3, 18, 8, 1, 0),
            new DateTime(2013, 3, 18, 9, 2, 0),
            new DateTime(2013, 3, 18, 10, 3, 0),
            new DateTime(2013, 3, 18, 11, 4, 0),
            new DateTime(2013, 3, 18, 12, 5, 0),
        };
        var dayTwoPasses = new[]
        {
            new DateTime(2013, 3, 19, 7, 0, 0),
            new DateTime(2013, 3, 19, 8, 30, 0),
        };

        Assert.Equal(86, _calculator.GetTollFee(vehicle, dayOnePasses.Concat(dayTwoPasses).ToArray()));
    }

    [Fact]
    public void GetTollFee_ResetsRollingWindow_OnNewCalendarDay()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            new DateTime(2013, 3, 18, 23, 30, 0),
            new DateTime(2013, 3, 19, 0, 15, 0),
        };

        Assert.Equal(0, _calculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_SortsUnorderedPasses_BeforeCalculating()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            Weekday.ToDateTime(new TimeOnly(8, 30)),
            Weekday.ToDateTime(new TimeOnly(7, 0))
        };

        Assert.Equal(26, _calculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_UsesInjectedPolicies()
    {
        var tollFeePolicy = new FixedFeePolicy(11);
        var vehicleExemptionPolicy = new VehicleExemptionPolicy();
        var holidayCalendar = new SwedishHolidayCalendar();
        var calculator = new TollCalculator(tollFeePolicy, vehicleExemptionPolicy, holidayCalendar);
        var vehicle = new Car();
        var date = Weekday.ToDateTime(new TimeOnly(12, 0));

        Assert.Equal(11, calculator.GetTollFee(date, vehicle));
    }

    private sealed class FixedFeePolicy(int fee) : ITollFeePolicy
    {
        public int GetFee(DateTime date) => fee;
    }
}
