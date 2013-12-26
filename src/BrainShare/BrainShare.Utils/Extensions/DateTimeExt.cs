using System;

namespace BrainShare.Utils.Extensions
{
    public static class DateTimeExt
    {
        private const string TimeAgoPrefix = "";

        public static string ToRelativeDate(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return string.Format("{0} секунд назад", timeSpan.Seconds);

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? String.Format(TimeAgoPrefix + "{0} минут назад", timeSpan.Minutes) : TimeAgoPrefix + "минуту назад";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? String.Format(TimeAgoPrefix + "{0} часов назад", timeSpan.Hours) : TimeAgoPrefix + "час назад";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? String.Format(TimeAgoPrefix + "{0} дней назад", timeSpan.Days) : "вчера";

            if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Days > 30 ? String.Format(TimeAgoPrefix + "{0} месяцев назад", timeSpan.Days / 30) : TimeAgoPrefix + "месяц";

            return timeSpan.Days > 365 ? String.Format(TimeAgoPrefix + "{0} лет назад", timeSpan.Days / 365) : TimeAgoPrefix + "в прошлом году";
        }

        public static String GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}