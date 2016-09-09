using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace Common.Wpf.Converters
{
    public abstract class SingletonValueConverter<T> : IValueConverter where T : IValueConverter, new()
    {
        #region Static Properties

        private static readonly IValueConverter _instance = new T();

        public static IValueConverter Instance
        {
            get { return _instance; }
        }

        #endregion Static Properties

        #region Overridable IValueConverter Implementation

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Overridable IValueConverter Implementation
    }

    public class AsTypeValueConverter : SingletonValueConverter<AsTypeValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var valueType = value.GetType();

                var type = parameter as Type;

                if (type != null && type.IsAssignableFrom(valueType))
                {
                    return value;
                }
                if (targetType.IsAssignableFrom(valueType))
                {
                    return value;
                }
            }
            return null;
        }
    }

    public class IsTypeValueConverter : SingletonValueConverter<IsTypeValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var type = parameter as Type;
                if (type != null)
                {
                    value = type.IsAssignableFrom(value.GetType());
                }
            }

            return value;
        }
    }

    public class NullToDoubleConverter : SingletonValueConverter<NullToDoubleConverter>
    {
        private readonly static System.ComponentModel.DoubleConverter _typeConverter = new System.ComponentModel.DoubleConverter();

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null ? 0.0d : _typeConverter.ConvertTo(value, targetType);
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (!(value is double) || value == null) ? 0.0d : value;
            return result;
        }
    }
}
