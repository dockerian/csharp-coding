using System;
using System.Globalization;
using System.Text;

namespace Common.Extensions
{    
    /// <summary>
    /// Extension methods for converting between binary byte arrays and hex strings.
    /// </summary>
    public static class FormatExtensions
    {
        /// <summary>
        /// Extends the byte array ToString method that converts the value of the current byte array to
        /// its equivalent string representation using the specified format.
        /// </summary>
        /// <param name="instance">The instance of the byte array.</param>
        /// <param name="format">A numeric format string.</param>
        /// <returns>The string representation of the current byte array, formatted as specified by the format parameter.</returns>
        /// <remarks>
        /// The format parameter can be either a standard or a custom numeric format string. All standard numeric
        /// format strings other than "R" (or "r") are supported, as are all custom numeric format characters.
        /// If format is null or an empty string (""), the return value is formatted with the general 
        /// numeric format specifier ("G").
        /// </remarks>
        public static string ToString(this byte[] instance, string format)
        {
            StringBuilder sb = new StringBuilder(instance.Length * 3);
            foreach(byte b in instance)
            {
                sb.Append(b.ToString(format, CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }        

        /// <summary>
        /// Extends the byte array with a method that converts the value of the current byte array to
        /// its equivalent string representation in hex format. This is equivilent to the hex numeric format specifier ("X2").
        /// </summary>
        /// <param name="instance">The instance of the byte array.</param>
        /// <returns>The string representation of the current byte array, formatted as hex.</returns>
        public static string ToHex(this byte[] instance)
        {
            return instance.ToHex(0, instance.Length);            
        }

        /// <summary>
        /// Extends the byte array with a method that converts a range of bytes from the current byte array to
        /// its equivalent string representation in hex format. This is equivilent to the hex numeric format specifier ("X2").
        /// </summary>
        /// <param name="instance">The instance of the byte array.</param>
        /// <param name="startIndex">A value that represents the index at which the conversion begins.</param>
        /// <param name="length">A value that represents the number of elements to convert.</param>
        /// <returns>The string representation of the specified range of the byte array, formatted as hex.</returns>
        public static string ToHex(this byte[] instance, int startIndex, int length)
        {
            StringBuilder sb = new StringBuilder(length * 3);

            for(int i = startIndex; i < startIndex + length; ++i)
            {
                sb.Append(instance[i].ToString("X2", CultureInfo.InvariantCulture)); 
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extends the byte array with a method that converts a range of bytes from the current byte array to
        /// its equivalent string representation in hex format. Each byte is seperated by the specified delimiter.
        /// </summary>
        /// <param name="instance">The instance of the byte array.</param>
        /// <param name="startIndex">A value that represents the index at which the conversion begins.</param>
        /// <param name="length">A value that represents the number of elements to convert.</param>
        /// <param name="delimiter">The character used to seperate the bytes in the result.</param>
        /// <returns>The string representation of the specified range of the byte array, formatted as hex with each byte seperated by the specified delimiter.</returns>
        public static string ToHex(this byte[] instance, int startIndex, int length, char delimiter)
        {
            StringBuilder sb = new StringBuilder(length * 3);

            for(int i = startIndex; i < startIndex + length; ++i)
            {
                if (i != startIndex)
                    sb.Append(delimiter);
                sb.Append(instance[i].ToString("X2", CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extends the byte array with a method that converts the value of the current byte array to
        /// its equivalent string representation in hex format.
        /// </summary>
        /// <param name="instance">The instance of the byte array.</param>
        /// <param name="delimiter">The character used to seperate the bytes in the result.</param>
        /// <returns>The string representation of the byte array, formatted as hex with each byte seperated by the specified delimiter.</returns>
        public static string ToHex(this byte[] instance, char delimiter)
        {
            return instance.ToHex(0, instance.Length, delimiter);
        }
        
        /// <summary>
        /// Extends the byte array with a method that converts the value of the current byte array to
        /// its equivalent string representation in base64 format.
        /// </summary>
        /// <param name="instance">The instance of the byte array.</param>
        /// <returns>The string representation of the byte array, formatted in base64.</returns>
        public static string ToBase64(this byte[] instance)
        {
            return Convert.ToBase64String(instance);
        }

        /// <summary>
        /// Extends the <see cref="String"/> class with a method that converts a hex string to 
        /// its equivilent byte array.
        /// </summary>
        /// <param name="value">The instance of the string.</param>
        /// <returns>An array of 8-bit unsigned integers equivalent to the string instance.</returns>
        public static byte[] GetHexBytes(this string value)
        {
            byte[] result = new byte[value.Length / 2];

            for(int i = 0; i < result.Length; i++)
                result[i] = byte.Parse(value.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);

            return result;
        }

        /// <summary>
        ///  Extends the <see cref="String"/> class with a method that converts the String, which encodes binary
        ///  data as base 64 digits, to an equivalent byte array.
        /// </summary>
        /// <param name="value">The instance of the string.</param>
        /// <returns>An array of 8-bit unsigned integers equivalent to the string instance.</returns>
        public static byte[] GetBase64Bytes(this string value)
        {
            return Convert.FromBase64String(value);
        }

        /// <summary>
        /// Extends the <see cref="String"/> class with a method that reverses all of the characters in the string.
        /// 
        /// See http://weblogs.asp.net/justin_rogers/archive/2004/06/10/153175.aspx
        /// </summary>
        /// <param name="value">The instance of the string.</param>
        /// <returns>A string with all characters reversed from the specified string.</returns>
        public static string Reverse(this string value)
        {
            if (value == null) return null;
            char[] reversed = value.ToCharArray();
            Array.Reverse(reversed);
            return new String(reversed);
        }
        
        /// <summary>
        /// Extends the <see cref="String"/> class with a method that checks for an empty string using the best
        /// pratices for string checking.
        /// 
        /// See http://msdn.microsoft.com/en-us/library/ms182279.aspx.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to be checked.</param>
        /// <returns>true if the value parameter is an empty string (""); otherwise, false.</returns>
        public static bool IsEmpty(this string value) 
        {
            return (value.Length == 0);
        }

        /// <summary>
        /// Extends the <see cref="String"/> class with a method that checks for a null or empty string. This is
        /// an extension of the standard IsNullOrEmpty method to make code more readable.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to be checked.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Extends the <see cref="String"/> class with a method that checks for a null or empty string. This is
        /// an extension of the standard IsNullOrWhiteSpace method to make code more readable.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to be checked.</param>
        /// <returns>true if the value parameter is null, an empty string (""), or whitespace; otherwise, false.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }              
       
        /// <summary>
        /// Extends the <see cref="String"/> class with a method that strips a substring.
        /// </summary>
        /// <param name="value">The instance of the string.</param>
        /// <param name="stripText">The text to be removed from the string.</param>
        /// <returns>A string with the specified text removed.</returns>
        public static string Strip(this string value, string stripText)
        {
            return value.Replace(stripText, String.Empty);
        }

        /// <summary>
        /// Extends the <see cref="Guid"/> class with a method that checks for an empty Guid value.
        /// </summary>
        /// <param name="guid">The <see cref="Guid"/> to be checked.</param>
        /// <returns>true if the value parameter is all zeroes; otherwise, false.</returns>
        public static bool IsEmpty(this Guid value)
        {
            return (value == Guid.Empty);
        }
        
    }    
}
