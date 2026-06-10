using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public sealed class TollCalculator
{
    public const int DailyMaximumFee = 60;
    public const int HourlyWindowMinutes = 60;

    private readonly ITollFeePolicy _tollFeePolicy;
    private readonly IVehicleExemptionPolicy _vehicleExemptionPolicy;
    private readonly IHolidayCalendar _holidayCalendar;

    public TollCalculator()
        : this(new TollFeePolicy(), new VehicleExemptionPolicy(), new SwedishHolidayCalendar())
    {
    }

    public TollCalculator(
        ITollFeePolicy tollFeePolicy,
        IVehicleExemptionPolicy vehicleExemptionPolicy,
        IHolidayCalendar holidayCalendar)
    {
        _tollFeePolicy = tollFeePolicy ?? throw new ArgumentNullException(nameof(tollFeePolicy));
        _vehicleExemptionPolicy = vehicleExemptionPolicy ?? throw new ArgumentNullException(nameof(vehicleExemptionPolicy));
        _holidayCalendar = holidayCalendar ?? throw new ArgumentNullException(nameof(holidayCalendar));
    }

    public int GetTollFee(IVehicle vehicle, params DateTime[] dates)
    {
        ArgumentNullException.ThrowIfNull(dates);

        if (dates.Length == 0)
            return 0;

        ArgumentNullException.ThrowIfNull(vehicle);

        if (_vehicleExemptionPolicy.IsExempt(vehicle))
            return 0;

        return dates
            .GroupBy(date => date.Date)
            .OrderBy(group => group.Key)
            .Sum(group => CalculateDailyFee(vehicle, group.OrderBy(date => date).ToArray()));
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (_vehicleExemptionPolicy.IsExempt(vehicle) || _holidayCalendar.IsTollFreeDate(date))
            return 0;

        return _tollFeePolicy.GetFee(date);
    }

    private int CalculateDailyFee(IVehicle vehicle, DateTime[] dayDates)
    {
        var intervalStart = dayDates[0];
        var maxFeeInInterval = GetTollFee(intervalStart, vehicle);
        var dailyFee = 0;

        for (var i = 1; i < dayDates.Length; i++)
        {
            var date = dayDates[i];
            var minutes = (date - intervalStart).TotalMinutes;
            var fee = GetTollFee(date, vehicle);

            if (minutes <= HourlyWindowMinutes)
            {
                maxFeeInInterval = Math.Max(maxFeeInInterval, fee);
            }
            else
            {
                dailyFee += maxFeeInInterval;
                intervalStart = date;
                maxFeeInInterval = fee;
            }
        }

        dailyFee += maxFeeInInterval;
        return Math.Min(dailyFee, DailyMaximumFee);
    }
}
