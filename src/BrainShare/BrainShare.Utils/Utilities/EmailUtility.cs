using System.Net.Mail;
using System.Text.RegularExpressions;
using BrainShare.Utils.Extensions;

namespace BrainShare.Utils.Utilities
{
    public static class EmailUtility
    {
        public static readonly Regex EmailRegex = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);

        public static string GetTenantName(string email)
        {
            Ensure.That(IsEmailAddress(email),string.Format(@"Email ""{0}"" is not valid.",email));
            return ExtractTenantName(email);
        }

        public static bool IsEmailAddress(this string @string)
        {
            return @string.HasValue() && EmailRegex.IsMatch(@string);
        }

        private static string ExtractTenantName(string email)
        {
            var address = new MailAddress(email);
            var dotIndex = address.Host.LastIndexOf('.');

            return address.Host.Substring(0,  dotIndex);
        }
    }
}
