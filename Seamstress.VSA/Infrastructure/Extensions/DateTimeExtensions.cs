using System;

namespace Seamstress.VSA.Infrastructure.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo LocalZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/El_Salvador");

    public static (DateTimeOffset startUtc, DateTimeOffset endUtc) GetCurrentWeekUtcRange()
    {
        //Get UTC and get Local Date
        var utcNow = DateTimeOffset.UtcNow;
        var localNow = TimeZoneInfo.ConvertTime(utcNow, LocalZone);

        //Calculate how many days have passed since monday
        int daysFromMonday = ((int)localNow.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;

        //Calculate local start and End of the Week 
        var localWeekStart = localNow.Date.AddDays(-daysFromMonday);
        var localWeekEnd = localWeekStart.AddDays(7);

        //Get Offset from start and end
        var startOffset = LocalZone.GetUtcOffset(localWeekStart);
        var endOffset = LocalZone.GetUtcOffset(localWeekEnd);

        //Get Utc start and end of the week from Offset
        var startUtc = new DateTimeOffset(localWeekStart, startOffset).ToUniversalTime();
        var endUtc = new DateTimeOffset(localWeekEnd, endOffset).ToUniversalTime();

        return (startUtc, endUtc);
    }

    public static (DateTimeOffset StartUtc, DateTimeOffset EndUtc) ToUtcRange(
        this DateTimeOffset clientStart, DateTimeOffset clientEnd)
    {
        return (
            clientStart.ToUniversalTime(),
            clientEnd.ToUniversalTime()
        );
    }

    public static (DateTimeOffset Start, DateTimeOffset End) GetMonthRangeUtc(
        int month, int year)
    {
        // Primer día del mes a medianoche en hora local
        var localMonthStart = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Unspecified);

        // Primer día del MES SIGUIENTE a medianoche (límite exclusivo)
        var localMonthEnd = localMonthStart.AddMonths(1);

        var offsetStart = LocalZone.GetUtcOffset(localMonthStart);
        var offsetEnd = LocalZone.GetUtcOffset(localMonthEnd);

        return (
            new DateTimeOffset(localMonthStart, offsetStart).ToUniversalTime(),
            new DateTimeOffset(localMonthEnd, offsetEnd).ToUniversalTime()
        );
    }
}
