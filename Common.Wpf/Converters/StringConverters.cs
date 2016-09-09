using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;


namespace Common.Wpf.Converters
{
    [ValueConversion(typeof(String), typeof(Boolean))]
    public class StringNotNullOrEmptyConverter : IValueConverter
    {
        private static readonly StringNotNullOrEmptyConverter _instance = new StringNotNullOrEmptyConverter();

        public static StringNotNullOrEmptyConverter Instance
        {
            get { return _instance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value != null && !string.IsNullOrEmpty(value.ToString());
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class StringRepetitionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String single = value as String;
            String result = String.Empty;

            if (String.IsNullOrEmpty(single)) return String.Empty;

            int repetition = 1;

            if (parameter != null)
            {
                try
                {
                    repetition = Int32.Parse((string)parameter);
                }
                catch { }
            }

            if (repetition == 0)
            {
                result = new String(' ', single.Length);
            }
            else if (repetition > 1)
            {
                result = single;

                for(int i = 1; i < repetition; i++)
                {
                    result += single;
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String result = value as String;

            if (String.IsNullOrEmpty(result)) return String.Empty;

            int repetition = 1;

            if (parameter != null)
            {
                try
                {
                    repetition = Int32.Parse((string)parameter);
                }
                catch { }
            }

            if (repetition == 0)
            {
                result = new String(' ', result.Length);
            }
            else if (repetition > 1)
            {
                int len = Math.Abs(result.Length / repetition);

                if (len > 0)
                {
                    result = result.Substring(0, len);
                }
            }
            return result;
        }
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class StringToEllipsisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String result = value as String;

            if (String.IsNullOrEmpty(result)) return String.Empty;

            int repetition = 1;

            if (parameter != null)
            {
                try
                {
                    repetition = Int32.Parse((string)parameter);
                }
                catch { }
            }

            if (repetition == 0)
            {
                result = new String(' ', result.Length);
            }
            else if (repetition > 0)
            {
                string r = String.Empty;

                for (int i = 0, x = 1; i < result.Length; i++, x++)
                {
                    if (x <= repetition)
                    {
                        r += "."; i++;
                    }
                    r += " ";
                }
                result = r;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String result = value as String;

            if (String.IsNullOrEmpty(result)) return String.Empty;

            return result = new String(' ', result.Length);
        }
    }

}
