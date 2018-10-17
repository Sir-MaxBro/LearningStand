using System;
using System.Text;

namespace Stand.Domain.Extensions
{
    public static class CommandExtension
    {
        public static string GetAfter(this string value, char @char)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("String is null or empty");
            }

            var result = value.Remove(0, value.IndexOf(@char) + 1);
            return result;
        }

        public static string Except(this string value, params char[] chars)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("String is null or empty");
            }

            var result = new StringBuilder(value);
            foreach (var @char in chars)
            {
                result.Replace(@char.ToString(), "");
            }

            return result.ToString();
        }
    }
}
