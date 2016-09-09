using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Wpf.Converters
{
    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class BooleanToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Visible; // Default non-True value results in Visible

            if (value != null &&
                (Boolean)value)
            {
                result = Visibility.Collapsed; // Default True value results in Collapsed

                if (parameter != null)
                {
                    if ((string)parameter == "Hidden")
                    {
                        result = Visibility.Hidden;
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean result = true; // Default non-True value results in False

            try
            {
                if (value != null && (Visibility)value != Visibility.Visible)
                {
                    result = false;
                }
            }
            finally { };

            return result;
        }
    }
}
