namespace Brainshare.Infrastructure.Platform.Extensions
{
    public static class GenericExt
    {
        public static string ToString<T>(this T source, string ifNullValue)
        {
            return source.Equals(default(T)) ? ifNullValue : source.ToString();
        }
    }
}