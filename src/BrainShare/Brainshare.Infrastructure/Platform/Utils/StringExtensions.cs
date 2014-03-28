using System;
using System.Collections.Generic;
using System.Linq;

namespace Brainshare.Infrastructure.Platform.Utils
{
    public static class CommonExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            var pos = 0;
            var sourceList = source.ToList();
            while (sourceList.Skip(pos).Any())
            {
                yield return sourceList.Skip(pos).Take(chunksize);
                pos += chunksize;
            }
        }

        //private const string TimeAgoPrefix = "about ";
        private const string TimeAgoPrefix = "";

        public static string ToRelativeDate(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return string.Format("{0} seconds ago", timeSpan.Seconds);

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? String.Format(TimeAgoPrefix +"{0} minutes ago", timeSpan.Minutes) : TimeAgoPrefix + "a minute ago";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? String.Format(TimeAgoPrefix + "{0} hours ago", timeSpan.Hours) : TimeAgoPrefix +  "an hour ago";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? String.Format(TimeAgoPrefix  +"{0} days ago", timeSpan.Days) : "yesterday";

            if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Days > 30 ? String.Format(TimeAgoPrefix + "{0} months ago", timeSpan.Days / 30) : TimeAgoPrefix + "a month ago";

            return timeSpan.Days > 365 ? String.Format(TimeAgoPrefix + "{0} years ago", timeSpan.Days / 365) : TimeAgoPrefix + "a year ago";
        }

        public static string ToYesNo(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}