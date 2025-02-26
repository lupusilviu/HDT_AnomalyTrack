using System;
using System.Globalization;
using System.Windows.Data;

namespace HDT_BGFightTracker
{
    public class ValueToPercentableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2
                 && values[0] is int
                 && values[1] is int)
            {
                double number = (int)values[0];
                double total = (int)values[1];

                return (number / total) * 100.0;
            }

            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimePassedSinceBinaryDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long)
            {
                long binaryValue = (long)value;
                DateTime previousDate = DateTime.FromBinary(binaryValue);
                TimeSpan diffTime = DateTime.Now - previousDate;
                if (binaryValue == 0)
                {
                    return "never";
                }
                else if (diffTime < TimeSpan.FromHours(1))
                {
                    return String.Format("{0:#,0.0}mins ago", diffTime.TotalMilliseconds);
                }
                else if (diffTime < TimeSpan.FromDays(60))
                {
                    return String.Format("{0:#,0.0}days ago", diffTime.TotalDays);
                }
                else
                {
                    return "a long time";
                }
            }

            return "never";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
