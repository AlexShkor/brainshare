
namespace System
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
    }
}
