using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;


namespace Common.Wpf.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public class BooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor = 1;

            if (parameter != null)
            {
                try
                {
                    factor = Math.Abs(Double.Parse((string)parameter));
                }
                catch { }
            }

            return (value != null && (Boolean)value) ? 1 * factor : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = Double.IsNaN((double)value) ? 0 : (double)value;

            return result > 0.0 ? true : false;
        }
    }

    [ValueConversion(typeof(bool), typeof(double))]
    public class BooleanToOpacityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor = 1;

            if (parameter != null)
            {
                factor = Math.Abs(Double.Parse((string)parameter));
            }

            return (value != null && (Boolean)value) ? 0 : 1 * factor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = Double.IsNaN((double)value) ? 0 : (double)value;

            return result > 0.0 ? false : true;
        }
    }

    [ValueConversion(typeof(int), typeof(double))]
    public class GreaterThanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int comparedValue = 0;

            if (parameter != null)
            {
                try
                {
                    comparedValue = int.Parse(parameter.ToString());
                }
                catch { }
            }

            if (value != null && (int)value > comparedValue)
            {
                return 1;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (double)value > 0.0)
                return (int)1;
            else
                return (int)0;
        }
    }

    [ValueConversion(typeof(int), typeof(double))]
    public class GreaterThanToOpacityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int comparedValue = 0;

            if (parameter != null)
            {
                try
                {
                    comparedValue = int.Parse(parameter.ToString());
                }
                catch { }
            }

            if (value != null && (int)value > comparedValue)
            {
                return 0;
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (double)value > 0.0)
                return (int)0;
            else
                return (int)1;
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    public class GreaterThanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int comparedValue = 0;

            if (parameter != null)
            {
                try
                {
                    comparedValue = int.Parse(parameter.ToString());
                }
                catch { }
            }

            if (value != null && (int)value > comparedValue)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int comparedValue = 1;

            if (parameter != null)
            {
                try
                {
                    comparedValue = int.Parse(parameter.ToString());
                }
                catch { }
            }

            if (value != null && value is Visibility && (Visibility)value == Visibility.Visible)
            {
                return ++comparedValue;
            }
            else
            {
                return --comparedValue;
            }
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    public class GreaterThanToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int comparedValue = 0;

            if (parameter != null)
            {
                try
                {
                    comparedValue = int.Parse(parameter.ToString());
                }
                catch { }
            }

            if (value != null && (int)value > comparedValue)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int comparedValue = 1;

            if (parameter != null)
            {
                try
                {
                    comparedValue = int.Parse(parameter.ToString());
                }
                catch { }
            }

            if (value != null && value is Visibility && (Visibility)value == Visibility.Visible)
            {
                return --comparedValue;
            }
            else
            {
                return ++comparedValue;
            }
        }
    }

    [ValueConversion(typeof(int), typeof(double))]
    public class IntValueToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor = 1;

            if (parameter != null)
            {
                factor = Math.Abs(Double.Parse((string)parameter));
            }

            double result = (value != null && ((int)value > 0)) ? factor : 0;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Double.IsNaN((double)value)) return 0;

            double opacity = (Double)value;

            return opacity > 0 ? 1 : 0;
        }
    }

    [ValueConversion(typeof(int), typeof(double))]
    public class IntValueToOpacityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor = 1;

            if (parameter != null)
            {
                factor = Math.Abs(Double.Parse((string)parameter));
            }

            double result = (value != null && ((int)value > 0)) ? 0 : factor;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Double.IsNaN((double)value)) return 0;

            double opacity = (Double)value;

            return opacity > 0 ? 0 : 1;
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class OpacityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Double.IsNaN((double)value)) return null;

            double opacity = 1 - (Double)value;

            return opacity > 0 ? opacity : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Double.IsNaN((double)value)) return null;

            double opacity = 1 - (Double)value;

            return opacity > 0 ? opacity : 0;
        }
    }

    [ValueConversion(typeof(double), typeof(Visibility))]
    public class OpacityToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility defaultResult = Visibility.Collapsed; // default visibility

            if (parameter != null && (string)parameter != null)
            {
                string parameterValue = ((string)parameter).ToLower();

                if (parameterValue == "hidden")
                {
                    defaultResult = Visibility.Hidden;
                }
            }

            if (Double.IsNaN((double)value)) return defaultResult;

            double opacity = (Double)value;

            return opacity > 0 ? defaultResult : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double result = 1;

            try {
                if (value != null && (Visibility)value == Visibility.Visible)
                {
                    result = 0;
                }
            }
            catch { };

            return result;
        }
    }

    [ValueConversion(typeof(double), typeof(Visibility))]
    public class OpacityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility defaultResult = Visibility.Collapsed; // default visibility

            if (parameter != null && (string)parameter != null)
            {
                string parameterValue = ((string)parameter).ToLower();

                if (parameterValue == "hidden")
                {
                    defaultResult = Visibility.Hidden;
                }
            }

            if (Double.IsNaN((double)value)) return defaultResult;

            double opacity = (Double)value;

            return opacity > 0 ? Visibility.Visible : defaultResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double result = 0;

            try {
                if (value != null && (Visibility)value == Visibility.Visible)
                {
                    result = 1;
                }
            }
            catch {};

            return result;
        }
    }

}
