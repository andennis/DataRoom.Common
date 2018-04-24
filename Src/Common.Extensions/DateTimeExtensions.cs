using System;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime TruncateMiliseconds(this DateTime dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond);
        }

        public static long ToUnixTimeSeconds(this DateTime dt)
        {
            return Convert.ToInt64((dt.TruncateMiliseconds().ToUniversalTime() - UnixEpoch).TotalSeconds);
        }

        public static DateTime UnixTimeSecondsToDateTime(this long seconds)
        {
            return UnixEpoch.AddSeconds(seconds);
        }
    }
}
