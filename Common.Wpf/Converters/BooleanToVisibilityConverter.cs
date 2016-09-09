using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Wpf.Converters
{
    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed; // Default non-True value results in Collapsed

            if (parameter != null)
            {
                if ((string)parameter == "Hidden")
                {
                    result = Visibility.Hidden;
                }
            }

            if (value != null && (Boolean)value)
            {
                result = Visibility.Visible; // Default visibility is Visible when value is True
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean result = false; // Default non-True value results in False

            try
            {
                if (value != null && (Visibility)value != Visibility.Visible)
                {
                    result = true;
                }
            }
            finally { };

            return result;
        }
    }
}
