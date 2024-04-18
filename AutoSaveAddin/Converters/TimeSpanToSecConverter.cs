using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoSaveAddin.Converters;

public class TimeSpanToSecConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TimeSpan spanValue = (TimeSpan)value;
        return spanValue.TotalSeconds.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string valueString = value as string;
        int valueInt = int.Parse(valueString);
        return new TimeSpan(0, 0, valueInt);
    }
}