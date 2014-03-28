namespace Brainshare.Infrastructure.Platform.Extensions
{
    public static class BoolExt
    {
        public static string ToYesNoString(this bool source)
        {
            return source ? "Yes" : "No";
        }
    }
}