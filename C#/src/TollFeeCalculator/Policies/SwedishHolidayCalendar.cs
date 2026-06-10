namespace TollFeeCalculator.Policies;

public sealed class SwedishHolidayCalendar : IHolidayCalendar
{
    private readonly Dictionary<int, HashSet<DateOnly>> _cache = new();

    public bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            return true;

        return GetHolidaysForYear(date.Year).Contains(DateOnly.FromDateTime(date));
    }

    private HashSet<DateOnly> GetHolidaysForYear(int year)
    {
        if (_cache.TryGetValue(year, out var holidays))
            return holidays;

        holidays = year switch
        {
            2013 =>
            [
                new DateOnly(2013, 1, 1),
                new DateOnly(2013, 3, 28),
                new DateOnly(2013, 3, 29),
                new DateOnly(2013, 4, 1),
                new DateOnly(2013, 4, 30),
                new DateOnly(2013, 5, 1),
                new DateOnly(2013, 5, 8),
                new DateOnly(2013, 5, 9),
                new DateOnly(2013, 6, 5),
                new DateOnly(2013, 6, 6),
                new DateOnly(2013, 6, 21),
                new DateOnly(2013, 11, 1),
                new DateOnly(2013, 12, 24),
                new DateOnly(2013, 12, 25),
                new DateOnly(2013, 12, 26),
                new DateOnly(2013, 12, 31),
            ],
            _ => GetSwedishPublicHolidays(year).ToHashSet()
        };

        _cache[year] = holidays;
        return holidays;
    }

    private static IEnumerable<DateOnly> GetSwedishPublicHolidays(int year)
    {
        var easter = CalculateEasterSunday(year);

        yield return new DateOnly(year, 1, 1);
        yield return new DateOnly(year, 1, 6);
        yield return easter.AddDays(-3);
        yield return easter.AddDays(-2);
        yield return easter.AddDays(1);
        yield return new DateOnly(year, 5, 1);
        yield return easter.AddDays(39);
        yield return easter.AddDays(49);
        yield return new DateOnly(year, 6, 6);
        yield return GetMidsummerDay(year);
        yield return GetAllSaintsDay(year);
        yield return new DateOnly(year, 12, 24);
        yield return new DateOnly(year, 12, 25);
        yield return new DateOnly(year, 12, 26);
        yield return new DateOnly(year, 12, 31);
    }

    private static DateOnly CalculateEasterSunday(int year)
    {
        var a = year % 19;
        var b = year / 100;
        var c = year % 100;
        var d = b / 4;
        var e = b % 4;
        var f = (b + 8) / 25;
        var g = (b - f + 1) / 3;
        var h = (19 * a + b - d - g + 15) % 30;
        var i = c / 4;
        var k = c % 4;
        var l = (32 + 2 * e + 2 * i - h - k) % 7;
        var m = (a + 11 * h + 22 * l) / 451;
        var month = (h + l - 7 * m + 114) / 31;
        var day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateOnly(year, month, day);
    }

    private static DateOnly GetMidsummerDay(int year)
    {
        var date = new DateOnly(year, 6, 20);
        while (date.DayOfWeek != DayOfWeek.Saturday)
            date = date.AddDays(1);

        return date;
    }

    private static DateOnly GetAllSaintsDay(int year)
    {
        var date = new DateOnly(year, 10, 31);
        while (date.DayOfWeek != DayOfWeek.Saturday)
            date = date.AddDays(1);

        return date;
    }
}
