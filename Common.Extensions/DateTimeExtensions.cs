using System;

namespace Common.Extensions
{
    public enum MatrixRotationType
    {
        Clockwise = 90,
        Clockwise180 = 180,
        CounterClockwise = 270,
    }

    /// <summary>
    /// Extension methods for converting DateTimes and DAteTimeOffsets to other formats.
    /// </summary>
    public static class DateTimeExtensions
    {
        private static DateTimeOffset unixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        #region Convertion methods
        /// <summary>
        /// Creates a DateTimeOffset based on a Unix timestamp.
        /// </summary>
        /// <param name="timestamp">Unix representation based on the 1970 Unix Epoch.</param>
        /// <returns>The DateTimeOffset equivalent of the Unix timestamp.</returns>
        public static DateTimeOffset FromUnixTime(double timestamp)
        {
            return unixEpoch.AddSeconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// Extends the DateTimeOffset with a ToUnixTime method that converts the value of the current DateTimeOffset to
        /// its equivalent Unix representation based on the 1970 Unix Epoch.
        /// </summary>
        /// <param name="instance">The instance of the DateTimeOffset.</param>        
        /// <returns>The number of seconds since Jan 1, 1970 GMT.</returns>
        /// <remarks>
        /// If you are reading this in the year 2038 then C# has not changed .
        /// </remarks>
        public static double ToUnixTime(this DateTimeOffset instance)
        {            
            TimeSpan diff = instance.ToUniversalTime() - unixEpoch;
            return Math.Floor(diff.TotalSeconds);
        }

        #endregion

        #region GetAngle* methods

        /// <summary>
        /// Get angle between the Hour and the Minute hands
        /// </summary>
        /// <param name="dateTime">specified date time (only Hour, Minute, and Second count)</param>
        /// <param name="ignoringSecond">ignoring angle by the Second hand</param>
        /// <param name="clockDialBy24Hours">using 24 hours clock dial</param>
        /// <returns>angle between the Hour and the Minute hands</returns>
        public static double GetAngle(this DateTime dateTime, bool ignoringSecond = false, bool angleDirection = false, bool clockDialBy24Hours = false)
        {
            double hrsFraction = clockDialBy24Hours ? 24 : 12;
            double angleSecond = 360 * dateTime.Second / 60.0;
            double angleMinute = 360 * dateTime.Minute / 60.0 + (ignoringSecond ? 0 : angleSecond / 60.0);
            double angleHour = ((360 * (dateTime.Hour % hrsFraction)) + angleMinute) / hrsFraction;
            double angle = angleMinute - angleHour;

            return angleDirection ? angle : Math.Abs(angle);
        }
        public static double GetAngleOfHourHand(this DateTime dateTime, bool ignoringSecond = false, bool clockDialBy24Hours = false)
        {
            double hrsFraction = clockDialBy24Hours ? 24 : 12;
            double angleSecond = 360 * dateTime.Second / 60.0;
            double angleMinute = 360 * dateTime.Minute / 60.0 + (ignoringSecond ? 0 : angleSecond / 60.0);
            double angleHour = ((360 * (dateTime.Hour % hrsFraction)) + angleMinute) / hrsFraction;

            return angleHour;
        }
        public static double GetAngleOfMinuteHand(this DateTime dateTime, bool ignoringSecond = false)
        {
            double angleSecond = 360 * dateTime.Second / 60.0;
            double angleMinute = 360 * dateTime.Minute / 60.0 + (ignoringSecond ? 0 : angleSecond / 60.0);

            return angleMinute;
        }

        #endregion

        #region UTCTime methods

        public static String GetTimeSpan(this String timeSpanValue)
        {
            try
            {
                int duration = Int32.Parse(timeSpanValue);
                TimeSpan ts = new TimeSpan(0, 0, duration);
                return ts.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
            return String.Empty;
        }

        public static DateTime ParseUtcTime(long utcTime)
        {
            System.TimeSpan diff_utc = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixEpochTime = System.Convert.ToInt64(diff_utc.TotalSeconds);
            long diff = unixEpochTime - utcTime;
            DateTime epochStart = new DateTime(1970, 1, 1);
            DateTime dt = DateTime.Now.Subtract(new TimeSpan(0, 0, (int)diff));
            // dt = epochStart.AddSeconds(0 - diff);
            return dt;
        }

        public static DateTime ParseUtcTime(string utcTimeTag)
        {
            if (String.IsNullOrEmpty(utcTimeTag)) return DateTime.Now;

            long utcTime;

            try
            {
                utcTime = long.Parse(utcTimeTag);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                return DateTime.Now;
            }
            return ParseUtcTime(utcTime);
        }

        public static DateTimeOffset ParseUtcTimeToDateTimeOffset(string utcTime)
        {
            return FromUnixTime(double.Parse(utcTime));
        }

        public static DateTime ParseUtcTimeString(string utcTime)
        {
            DateTime utcDateTime = DateTime.Now;

            if (String.IsNullOrEmpty(utcTime)) return utcDateTime;

            try
            {
                utcDateTime = DateTime.Parse(utcTime);
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
                utcDateTime = utcDateTime.ToLocalTime();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            return utcDateTime;
        }

        public static String ParseUtcToTimeString(string utcTimeTag)
        {
            if (String.IsNullOrEmpty(utcTimeTag) == false)
            {
                try
                {
                    long utcTime = long.Parse(utcTimeTag);

                    return ParseUtcToTimeString(utcTime);
                }
                catch { }
            }
            return string.Empty;
        }

        public static String ParseUtcToTimeString(long utcTime)
        {
            return ParseUtcTime(utcTime).ToLongTimeString();
        }

        #endregion
    }
}
