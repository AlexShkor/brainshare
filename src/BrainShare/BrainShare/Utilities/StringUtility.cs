using BrainShare.Documents;

namespace BrainShare.Utilities
{
    public static class StringUtility
    {
        public static string GetUserStatus(BaseUser user)
        {
            return user.IsOnline ? "Online" : user.LastVisited.ToString("ddd d MMM");
        }
    }
}