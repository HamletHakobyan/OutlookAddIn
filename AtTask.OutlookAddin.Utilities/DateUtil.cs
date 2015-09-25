using System;
using System.Globalization;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class DateUtil
    {
        public static readonly string DateFormatMonthDayYear = ShortDatePattern;
        public static readonly string DateFormatMonthDayYearHour = ShortDatePattern + " h:mm:ss tt";

        public const string InternetTimeFormat = "yyyy-MM-dd\\THH:mm:ss:fffzzz";
        public const string InternetShortDateFormat = "yyyy-MM-dd";
        public static readonly string ShortDateFormat = ShortDatePattern;

        public static bool TryParseInternetTime(string dateTimeString, out DateTime dateTime)
        {
            return DateTime.TryParseExact(dateTimeString, InternetTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
        }

        public static bool TryParseShortDate(string dateString, out DateTime dateTime)
        {
            return DateTime.TryParseExact(dateString, InternetShortDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
        }

        public static string GetInternetTimeString(DateTime dateTime, CultureInfo cultureInfo = null)
        {
            return dateTime.ToString(InternetTimeFormat, (cultureInfo != null) ? cultureInfo : CultureInfo.InvariantCulture);
        }

        public static string ShortDatePattern
        {
            get
            {
                if (CultureInfo.CurrentCulture.ToString() == "en-US" || CultureInfo.CurrentCulture.ToString() == "en-GB")
                {
                    return "MMM d, yyyy";
                }
                else
                {
                    return GetCurrentDateFormat().ShortDatePattern;
                }
                
            }
        }

        public static string DateWihtoutYearPattern
        {
            get
            {
                return GetShortDateFormatWithoutYear();
            }
        }


        public static DateTimeFormatInfo GetCurrentDateFormat()
        {
            return GetDateFormat(CultureInfo.CurrentCulture);
        }

        public static DateTimeFormatInfo GetDateFormat(CultureInfo culture)
        {
            if (culture.Calendar is GregorianCalendar)
            {
                return culture.DateTimeFormat;
            }
            else
            {
                GregorianCalendar foundCal = null;
                DateTimeFormatInfo dtfi = null;

                foreach (System.Globalization.Calendar cal in culture.OptionalCalendars)
                {
                    if (cal is GregorianCalendar)
                    {
                        // Return the first Gregorian calendar with CalendarType == Localized
                        // Otherwise return the first Gregorian calendar
                        if (foundCal == null)
                        {
                            foundCal = cal as GregorianCalendar;
                        }

                        if (((GregorianCalendar)cal).CalendarType == GregorianCalendarTypes.Localized)
                        {
                            foundCal = cal as GregorianCalendar;
                            break;
                        }
                    }
                }

                if (foundCal == null)
                {
                    // if there are no GregorianCalendars in the OptionalCalendars list, use the invariant dtfi
                    dtfi = ((CultureInfo)CultureInfo.InvariantCulture.Clone()).DateTimeFormat;
                    dtfi.Calendar = new GregorianCalendar();
                }
                else
                {
                    dtfi = ((CultureInfo)culture.Clone()).DateTimeFormat;
                    dtfi.Calendar = foundCal;
                }

                return dtfi;
            }
        }

        public static string GetDateWihtoutYear(DateTime dateTime)
        {
            string shortFormat = DateWihtoutYearPattern;
            string currentDate = dateTime.ToString(shortFormat);

            return currentDate;
        }

        public static string GetShortDateFormatWithoutYear()
        {
            string shortFormat = ShortDatePattern;
            string dateSeparator = GetCurrentDateFormat().DateSeparator.Trim();
            if (CultureInfo.CurrentCulture.ToString() == "en-US" || CultureInfo.CurrentCulture.ToString() == "en-GB")
            {
                dateSeparator = ",";
            }

            shortFormat = shortFormat.Replace("y", string.Empty).Replace("Y", string.Empty).Trim();
            shortFormat = shortFormat.Replace(dateSeparator + dateSeparator, string.Empty).Trim();
            shortFormat = shortFormat.Trim(dateSeparator.ToCharArray());

            if (shortFormat.EndsWith(dateSeparator))
            {
                shortFormat = shortFormat.Substring(0, shortFormat.Length - 1);
            }
            if (shortFormat.StartsWith(dateSeparator))
            {
                shortFormat = shortFormat.Substring(1, shortFormat.Length - 1);
            }

            return shortFormat;
        }

        public static DateTime? Max(params DateTime?[] dates)
        {

            DateTime? maxDate = null;

            foreach (var date in dates)
            {
                if (date.HasValue)
                {
                    maxDate = (date > maxDate || maxDate == null) ? date : maxDate;
                }
            }

            //if( dates == null || dates.Length == 0) return null;
            //if( dates.Length == 1 ) return dates[0];

            //DateTime max = new DateTime();

            //    foreach (DateTime date : dates) 
            //    {
            //    if( date == null ) continue;
            //    if( max.getTimeInMillis() < date.getTimeInMillis()) {
            //        max = date;
            //    }
            //}

            return maxDate;
        }
    }
}