using System;

namespace BrainShare.Utils.Utilities
{
    public static class StringUtility
    {
        public static string GetUserStatus(DateTime lastVisited, int userActivityTimeoutInMinutes)
        {
            var isUserOnline = userActivityTimeoutInMinutes > (DateTime.UtcNow - lastVisited).Minutes;
            return isUserOnline ? "Online" : lastVisited.ToString("ddd d MMM hh:mm");
        }
    }
}