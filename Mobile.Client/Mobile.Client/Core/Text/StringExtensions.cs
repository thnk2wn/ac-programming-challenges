using System.Text.RegularExpressions;

namespace AvantCredit.Uploader.Core.Text
{
    static class StringExtensions
    {
        public static bool IsAlphanumeric(this string source)
        {
            return (new Regex("^[a-zA-Z0-9]*$").IsMatch(source));
        }
    }
}