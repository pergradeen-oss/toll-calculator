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

        var sortedDates = dates.OrderBy(d => d).ToArray();
        var totalFee = 0;
        var intervalStart = sortedDates[0];
        var maxFeeInInterval = GetTollFee(intervalStart, vehicle);

        for (var i = 1; i < sortedDates.Length; i++)
        {
            var date = sortedDates[i];
            var minutes = (date - intervalStart).TotalMinutes;
            var fee = GetTollFee(date, vehicle);

            if (minutes <= HourlyWindowMinutes)
            {
                maxFeeInInterval = Math.Max(maxFeeInInterval, fee);
            }
            else
            {
                totalFee += maxFeeInInterval;
                intervalStart = date;
                maxFeeInInterval = fee;
            }
        }

        totalFee += maxFeeInInterval;
        return Math.Min(totalFee, DailyMaximumFee);
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (_vehicleExemptionPolicy.IsExempt(vehicle) || _holidayCalendar.IsTollFreeDate(date))
            return 0;

        return _tollFeePolicy.GetFee(date);
    }
}
