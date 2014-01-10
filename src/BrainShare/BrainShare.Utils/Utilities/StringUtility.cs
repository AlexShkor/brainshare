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

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetEmailSuffix(string email)
        {
            if (email == null)
            {
                return null;
            }

            var start = email.IndexOf("@");
            var end = email.LastIndexOf(".");

            if (start == -1 || end == -1 || start >= end)
            {
                return null;
            }
            start++;
            return email.Substring(start , end - start);
        }
    }
}