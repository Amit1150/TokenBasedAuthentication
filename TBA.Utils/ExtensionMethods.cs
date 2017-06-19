using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBA.Utils
{
   public static class ExtensionMethods
    {
        public static string ToFormateDateString(this DateTime dt)
        {
            var enCulture = new CultureInfo("en-US");
            return dt.ToString("dd-MMM-yyyy", enCulture);
        }

        public static string ToFormateDateTimeString(this DateTime dt)
        {
            var enCulture = new CultureInfo("en-US");
            return dt.ToString("dd-MMM-yyyy, hh:mm tt", enCulture);
        }

        public static string ToFormate24DateTimeString(this DateTime dt)
        {
            var enCulture = new CultureInfo("en-US");
            return dt.ToString("dd-MMM-yyyy, HH:mm:ss", enCulture);
        }

        public static long UnixTimeNow(this DateTime dt)
        {
            var timeSpan = (dt - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        public static DateTime ToSingapore(this DateTime dt)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            var date = TimeZoneInfo.ConvertTime(dt, timeZone);
            return date;
        }

        public static DateTime? ParseDate(this string date)
        {
            DateTime getDate = DateTime.Now;
            if (DateTime.TryParse(date, out getDate))
            {
                return getDate;
            }
            else
            {
                return null;
            }
        }

        public static string ToFileNameFormat(this DateTime dt)
        {
            var enCulture = new CultureInfo("en-US");
            return dt.ToString("yyyyMMddHHmmss", enCulture);
        }
    }
}
