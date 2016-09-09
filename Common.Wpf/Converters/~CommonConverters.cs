using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Wpf.Converters
{
    public static class CommonConverters
    {
        private static BooleanToInvisibilityConverter _booleanToInvisibilityConverter = null;
        private static BooleanToVisibilityConverter _booleanToVisibilityConverter = null;

        private static BooleanToOpacityConverter _booleanToOpacityConverter = null;
        private static BooleanToOpacityInverseConverter _booleanToOpacityInverseConverter = null;
        private static GreaterThanToOpacityConverter _greaterThanToOpacityConverter = null;
        private static GreaterThanToOpacityInverseConverter _greaterThanToOpacityInverseConverter = null;
        private static GreaterThanToVisibilityConverter _greaterThanToVisibilityConverter = null;
        private static GreaterThanToInvisibilityConverter _greaterThanToInvisibilityConverter = null;
        private static IntValueToOpacityConverter _intValueToOpacityConverter = null;
        private static IntValueToOpacityInverseConverter _intValueToOpacityInverseConverter = null;
        private static OpacityInverseConverter _opacityInverseConverter = null;
        private static OpacityToInvisibilityConverter _opacityToInvisibilityConverter = null;
        private static OpacityToVisibilityConverter _opacityToVisibilityConverter = null;

        private static StringNotNullOrEmptyConverter _stringNotNullOrEmptyConverter = null;
        private static StringRepetitionConverter _stringRepetitionConverter = null;
        private static StringToEllipsisConverter _stringToEllipsisConverter = null;

        #region Static Properties

        #region Boolean Converters

        public static BooleanToInvisibilityConverter BooleanToInvisibilityConverter
        {
            get { return _booleanToInvisibilityConverter ?? new BooleanToInvisibilityConverter(); }
        }

        public static BooleanToVisibilityConverter BooleanToVisibilityConverter
        {
            get { return _booleanToVisibilityConverter ?? new BooleanToVisibilityConverter(); }
        }

        #endregion

        #region Mapping Converters

        private static readonly MappingConverter _mappingConverter = null;
        public static MappingConverter MappingConverter
        {
            get { return _mappingConverter ?? new MappingConverter(); }
        }

        #endregion

        #region Opactity Converters

        public static BooleanToOpacityConverter BooleanToOpacityConverter
        {
            get { return _booleanToOpacityConverter ?? new BooleanToOpacityConverter(); }
        }

        public static BooleanToOpacityInverseConverter BooleanToOpacityInverseConverter
        {
            get { return _booleanToOpacityInverseConverter ?? new BooleanToOpacityInverseConverter(); }
        }

        public static GreaterThanToOpacityConverter GreaterThanToOpacityConverter
        {
            get { return _greaterThanToOpacityConverter ?? new GreaterThanToOpacityConverter(); }
        }

        public static GreaterThanToOpacityInverseConverter GreaterThanToOpacityInverseConverter
        {
            get { return _greaterThanToOpacityInverseConverter ?? new GreaterThanToOpacityInverseConverter(); }
        }

        public static GreaterThanToVisibilityConverter GreaterThanToVisibilityConverter
        {
            get { return _greaterThanToVisibilityConverter ?? new GreaterThanToVisibilityConverter(); }
        }

        public static GreaterThanToInvisibilityConverter GreaterThanToInvisibilityConverter
        {
            get { return _greaterThanToInvisibilityConverter ?? new GreaterThanToInvisibilityConverter(); }
        }

        public static IntValueToOpacityConverter IntValueToOpacityConverter
        {
            get { return _intValueToOpacityConverter ?? new IntValueToOpacityConverter(); }
        }

        public static IntValueToOpacityInverseConverter IntValueToOpacityInverseConverter
        {
            get { return _intValueToOpacityInverseConverter ?? new IntValueToOpacityInverseConverter(); }
        }

        public static OpacityInverseConverter OpacityInverseConverter
        {
            get { return _opacityInverseConverter ?? new OpacityInverseConverter(); }
        }

        public static OpacityToInvisibilityConverter OpacityToInvisibilityConverter
        {
            get { return _opacityToInvisibilityConverter ?? new OpacityToInvisibilityConverter(); }
        }

        public static OpacityToVisibilityConverter OpacityToVisibilityConverter
        {
            get { return _opacityToVisibilityConverter ?? new OpacityToVisibilityConverter(); }
        }

        #endregion

        #region Singleton Converters

        private static AsTypeValueConverter _asTypeValueConverter = null;
        public static AsTypeValueConverter AsTypeValueConverter
        {
            get { return _asTypeValueConverter ?? new AsTypeValueConverter(); }
        }

        private static IsTypeValueConverter _isTypeValueConverter = null;
        public static IsTypeValueConverter IsTypeValueConverter
        {
            get { return _isTypeValueConverter ?? new IsTypeValueConverter(); }
        }

        private static NullToDoubleConverter _nullToDoubleConverter = null;
        public static NullToDoubleConverter NullToDoubleConverter
        {
            get { return _nullToDoubleConverter ?? new NullToDoubleConverter(); }
        }

        #endregion

        #region String Converters

        public static StringNotNullOrEmptyConverter StringNotNullOrEmptyConverter
        {
            get { return _stringNotNullOrEmptyConverter ?? new StringNotNullOrEmptyConverter(); }
        }

        public static StringRepetitionConverter StringRepetitionConverter
        {
            get { return _stringRepetitionConverter ?? new StringRepetitionConverter(); }
        }

        public static StringToEllipsisConverter StringToEllipsisConverter
        {
            get { return _stringToEllipsisConverter ?? new StringToEllipsisConverter(); }
        }

        #endregion

        #endregion

    }
}
