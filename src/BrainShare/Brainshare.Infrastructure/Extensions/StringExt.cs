
using System;

namespace Brainshare.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.String"/>
    /// </summary>
    public static class StringExt
    {
        public static bool HasValue(this String value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string CutString(this String value, int limit)
        {
            if (value != null && value.Length > limit)
            {
                var cutString = value.Substring(0, limit);
                return cutString + "...";
            }
            return value;
        }
    }
}
