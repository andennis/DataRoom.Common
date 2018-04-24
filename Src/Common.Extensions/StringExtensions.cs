using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceFirst(this string srcString, string oldValue, string newValue, int startIndex = 0)
        {
            if (oldValue == null)
                throw new ArgumentNullException("oldValue");
            if (oldValue.Length == 0)
                throw new ArgumentException("oldValue is empty");

            int i = srcString.IndexOf(oldValue, startIndex, StringComparison.Ordinal);
            if (i <= 0)
                return srcString;

            var sb = new StringBuilder(srcString.Substring(0, i));
            sb.Append(newValue);
            sb.Append(srcString.Substring(i + oldValue.Length));
            return sb.ToString();
        }

        //TODO it should be revised. Something was wrong with BlockCopy
        public static byte[] GetBytes(this string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;            
        }

        public static IEnumerable<int> ConvertToInts(this string separatedIds, string separator = ",")
        {
            if (separatedIds == null)
                throw new ArgumentNullException("separatedIds");
            if (separator == null)
                throw new ArgumentNullException("separator");

            return separatedIds.Trim().Split(new [] {separator}, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x));
        }

    }
}
