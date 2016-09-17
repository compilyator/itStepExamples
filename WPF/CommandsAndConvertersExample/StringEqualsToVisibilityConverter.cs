using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConvertersDemo
{
    public class StringEqualsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility)) return Visibility.Hidden;
            if (!(value is string)) return Visibility.Hidden;
            if (!(parameter is string)) return Visibility.Hidden;
            if (((string) value).Equals((string) parameter))
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}