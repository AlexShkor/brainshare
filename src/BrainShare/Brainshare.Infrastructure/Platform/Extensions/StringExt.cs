using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Brainshare.Infrastructure.Platform.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.String"/>
    /// </summary>
    public static class StringExt
    {
        public static string GetDescription(this String value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value : attribute.Description;
        }

        public static DateTime? ToNullableDateTime(this String value)
        {
            DateTime result;

            if (DateTime.TryParseExact(value,
                                       AppConstants.DateFormat,
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                       out result))
            {
                return result;
            }
            return null;
        }

        public static bool HasValue(this String value)
        {
            return !string.IsNullOrEmpty(value) && !value.Equals("null", StringComparison.InvariantCultureIgnoreCase);
        }

        public static T GetAttribute<T>(this String value) where T : Attribute
        {
            var fi = value.GetType().GetField(value);
            if (fi == null)
            {
                return null;
            }
            var attributes = (T[]) fi.GetCustomAttributes(typeof (T), false);

            return attributes.SingleOrDefault(x => x.GetType() == typeof (T));
        }

        public static readonly Regex UrlRegex =
            new Regex(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)",
                      RegexOptions.Compiled);

        public static readonly Regex EmailRegex =
            new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$",
                      RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex humps = new Regex("(?:^[a-zA-Z][^A-Z]*|[A-Z][^A-Z]*)");


        /// <summary>
        /// Separates a given string at the first instance of the specified character
        /// </summary>
        /// <param name="string">String to split</param>
        /// <param name="until">Character at which to split</param>
        /// <param name="rest">String value beyond the separator</param>
        /// <returns>String before the instance of the until character</returns>
        public static string SeparateAt(this string @string, char until, out string rest)
        {
            if (@string.IsNullOrEmpty())
            {
                rest = string.Empty;
                return string.Empty;
            }

            var indexOfChar = @string.IndexOf(until);

            if (indexOfChar < 0)
            {
                rest = string.Empty;
                return @string;
            }

            rest = @string.Substring(indexOfChar, @string.Length - indexOfChar);
            return @string.Substring(0, indexOfChar);
        }

        /// <summary>
        /// Separates a given string at the first instance of the specified character
        /// </summary>
        /// <param name="string">String to split</param>
        /// <param name="until">Character at which to split</param>
        /// <returns>String before the instance of the until character</returns>
        public static string SeparateAt(this string @string, char until)
        {
            string rest; // unused
            return @string.SeparateAt(until, out rest);
        }

        /// <summary>
        /// Determines if a given string is a URL
        /// </summary>
        /// <param name="string">Strign to test for URL match</param>
        /// <returns>True if value is a URL, false otherwise</returns>
        public static bool IsUrl(this string @string)
        {
            return @string.IsNotNullOrEmpty() && UrlRegex.IsMatch(@string);
        }

        /// <summary>
        /// Determines if a given string is an email address
        /// </summary>
        /// <param name="string">String to test for email address match</param>
        /// <returns>True if the value is an email addres, false otherwise</returns>
        public static bool IsEmailAddress(this string @string)
        {
            return @string.IsNotNullOrEmpty() && EmailRegex.IsMatch(@string);
        }

        /// <summary>
        /// Determines if a given string is null or empty string
        /// </summary>
        /// <param name="string">String to test</param>
        /// <returns>True if null or empty. False otherwise.</returns>
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }

        /// <summary>
        /// Determines if a given string is not null or empty string
        /// </summary>
        /// <param name="string">String to test</param>
        /// <returns>False if null or empty. True otherwise.</returns>
        public static bool IsNotNullOrEmpty(this string @string)
        {
            return !string.IsNullOrEmpty(@string);
        }

        /// <summary>
        /// Removes a specified string from a string
        /// </summary>
        /// <param name="string">String from which to remove substrings</param>
        /// <param name="substrings">Strings to remove from origal string</param>
        /// <returns>String without all substrings specified</returns>
        public static string Without(this string @string, params string[] substrings)
        {
            if (@string.IsNullOrEmpty() || substrings == null || substrings.Length == 0)
            {
                return string.Empty;
            }

            return substrings.Where(s => s.IsNotNullOrEmpty())
                             .Aggregate(@string, (orig, without) => orig.Replace(without, string.Empty));
        }


        public static string CamelFriendly(this string camel)
        {
            if (string.IsNullOrWhiteSpace(camel))
            {
                return "";
            }

            var matches = humps.Matches(camel).OfType<Match>().Select(m => m.Value);
            return matches.Any()
                       ? matches.Aggregate((a, b) => a + " " + b).TrimStart(' ')
                       : camel;
        }

        public static string Ellipsize(this string text, int characterCount)
        {
            return text.Ellipsize(characterCount, "...");
        }

        public static string Ellipsize(this string text, int characterCount, string ellipsis)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            if (characterCount < 0 || text.Length <= characterCount)
            {
                return text;
            }

            return Regex.Replace(text.Substring(0, characterCount + 1), @"\s+\S*$", "") + ellipsis;
        }

        public static string HtmlClassify(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            var friendlier = text.CamelFriendly();
            return Regex.Replace(friendlier, @"[^a-zA-Z]+", m => m.Index == 0 ? "" : "-").ToLowerInvariant();
        }


        public static string RemoveTags(this string html)
        {
            return string.IsNullOrEmpty(html)
                       ? ""
                       : Regex.Replace(html, "<[^<>]*>", "", RegexOptions.Singleline);
        }

        // not accounting for only \r (e.g. Apple OS 9 carriage return only new lines)
        public static string ReplaceNewLinesWith(this string text, string replacement)
        {
            return string.IsNullOrWhiteSpace(text)
                       ? ""
                       : Regex.Replace(text, @"(\r?\n)", replacement, RegexOptions.Singleline);
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static byte[] ToByteArray(this string hex)
        {
            return System.Linq.Enumerable.Range(0, hex.Length).
                          Where(x => 0 == x%2).
                          Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
                          ToArray();
        }

        public static List<string> SplitByLength(this string source, int lineLength)
        {
            return source.SplitByLength(lineLength, ' ');
        }

        public static List<string> SplitByLength(this string source, int lineLength, params char[] separators)
        {
            var arr = source.Split(separators ?? new char[] {' '});
            var result = new List<string>();
            var current = String.Empty;
            foreach (var s in arr)
            {
                if ((current + s).Length > lineLength)
                {
                    if (current.HasValue())
                    {
                        result.Add(current);
                        current = String.Empty;
                    }
                    else
                    {
                        result.Add(s);
                    }
                }
                if (current.HasValue())
                {
                    current += " ";
                }
                current += s;
            }
            if (current.HasValue())
            {
                result.Add(current);
            }
            return result;
        }

        public static List<string> SplitAsEmailsList(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<string>();
            }

            return value.Replace(" ", "").Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        // split "AutomaticTrackingSystem" to "Automatic Tracking System"
        public static string SplitOnUpperCase(this string value, string replaceString = " ")
        {
            var regex = new Regex(@"(?!^)(?=[A-Z])");
            return regex.Replace(value, replaceString);
        }

        public static bool? ToBool(this string yesNoString)
        {
            var value = yesNoString.ToLower();
            switch (value)
            {
                case "y":
                case "yes":
                    return true;

                case "n":
                case "no":
                    return false;

                default:
                    return null;
            }
        }

        public static string GetMimeType(this string fileName)
        {
            var mimeType = "application/unknown";
            var fileExtension = Path.GetExtension(fileName);
            if (fileName.HasValue() && fileExtension.HasValue())
            {
                switch (fileExtension)
                {
                    case ".pdf":
                        mimeType = "application/pdf";
                        break;

                    case ".docx":
                        mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;

                    case ".doc":
                        mimeType = "application/msword";
                        break;

                    case ".xlsx":
                        mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;

                    case ".xls":
                        mimeType = "application/vnd.xls";
                        break;

                    case ".png":
                        mimeType = "image/png";
                        break;

                    case ".flv":
                        mimeType = "video/x-flv";
                        break;

                    default:
                        var regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(fileExtension); // search windows registry
                        if (regKey != null && regKey.GetValue("Content Type") != null)
                            mimeType = regKey.GetValue("Content Type").ToString();
                        break;
                }
            }

            return mimeType;
        }

        public static string ToPartlyShownNumber(this string ssn)
        {
            return ssn.HasValue() ? string.Format("XXX-XX-{0}", ssn.Substring(ssn.Length - 4)) : string.Empty;
        }
    }
}