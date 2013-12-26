using System;
using BrainShare.Documents;
using BrainShare.Domain.Documents;

namespace BrainShare.Infrastructure.Utilities
{
    public static class StringUtility
    {
        public static string GetUserStatus(BaseUser user, int userActivityTimeoutInMinutes)
        {
            var isUserOnline = userActivityTimeoutInMinutes  > (DateTime.UtcNow - user.LastVisited).Minutes;
            return isUserOnline ? "Online" : user.LastVisited.ToString("ddd d MMM hh:mm");
        }
    }
}