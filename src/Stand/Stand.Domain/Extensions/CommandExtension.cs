using System;

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
    }
}
