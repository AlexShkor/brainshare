using System;

namespace Brainshare.Infrastructure.Platform.Extensions
{
    public static class DateTimeExt
    {
        public static int YearMonthHash(this DateTime date)
        {
            return date.Month + date.Year*100;
        }

        public static int GetHash(this DateTime date)
        {
            return date.Day + date.Month*100 + date.Year*100*100;
        }

        public static DateTime GetDate(this int dateHash)
        {
            var k = dateHash;
            var day = k%100;
            k = (k - day)/100;
            var month = k%100;
            k = (k - month)/100;
            var year = k;
            return new DateTime(year, month, day);
        }

        public static string ToDateString(this DateTime? date, string emptyValue = "")
        {
            return date.HasValue ? date.Value.ToString(AppConstants.DateFormat) : emptyValue;
        }

        public static string ToDateString(this DateTime date, string emptyValue = "")
        {
            return date != default(DateTime) ? date.ToString(AppConstants.DateFormat) : emptyValue;
        }
    }
}