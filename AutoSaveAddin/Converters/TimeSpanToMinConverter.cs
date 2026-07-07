using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoSaveAddin.Converters
{
    public class TimeSpanToMinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan spanValue = (TimeSpan)value;
            return spanValue.TotalMinutes.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueString = value as string;
            int valueInt;
            if (!int.TryParse(valueString, out valueInt) || valueInt < 1)
                valueInt = 1;

            return new TimeSpan(0, valueInt, 0);
        }
    }
}
