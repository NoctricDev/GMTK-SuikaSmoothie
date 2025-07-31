using System;

namespace CSharpTools.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Converts a DateTime value to a Unix Timestamp
        /// </summary>
        /// <remarks></remarks>
        public static int ToUnixTimestamp(this DateTime date)
        {
            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        ///     Converts a Unix Timestamp to a .NET DateTime value
        /// </summary>
        public static DateTime UnixTimestampToDateTime(this long unixTimeStamp)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
