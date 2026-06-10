namespace TollFeeCalculator.Policies;

public interface IHolidayCalendar
{
    bool IsTollFreeDate(DateTime date);
}
