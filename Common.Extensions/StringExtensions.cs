using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;


namespace Common.Extensions
{
    public static class StrngExtensions
    {
        private static readonly Regex defaultRegexPhoneNumber = new Regex(@"[^\d\!\,\*\#A-Za-z]");

        #region Char and String with StringToken

        public static bool IsClosingToken(this Char c)
        {
            String s = c.ToString();

            return StringToken.IsClosingToken(s);
        }
        public static bool IsClosingToken(this String s)
        {
            if (String.IsNullOrEmpty(s)) return false;

            return StringToken.IsClosingToken(s);
        }
        public static bool IsOpeningToken(this Char c)
        {
            String s = c.ToString();

            return StringToken.IsOpeningToken(s);
        }
        public static bool IsOpeningToken(this String s)
        {
            if (String.IsNullOrEmpty(s)) return false;

            return StringToken.IsOpeningToken(s);
        }

        #endregion

        #region String Extension Methods

        public static String CapitalFirstLetters(this String words)
        {
            if (String.IsNullOrEmpty(words)) return words;

            char[] array = words.ToCharArray();

            for(int i = 0; i < array.Length; i++)
            {
                bool isLeadingChar = (i == 0 || !char.IsLetter(array[i - 1]));

                if (isLeadingChar)
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
                else if (char.IsUpper(array[i]))
                {
                    array[i] = char.ToLower(array[i]);
                }
            }
            return new String(array);
        }

        public static String GetDifference(this String source, string target, bool ignoreCase = false, bool returnDiffInSource = true, int startIndex = 0)
        {
            if (startIndex > 0)
            {
                source = (startIndex >= source.Length) ? String.Empty : source.Substring(0, startIndex);
                target = (startIndex >= target.Length) ? String.Empty : target.Substring(0, startIndex);
            }
            if (String.IsNullOrEmpty(source)) return target;
            if (String.IsNullOrEmpty(target)) return source;

            int diffStartIndex = startIndex;

            string diff = String.Empty;

            for(int i = startIndex; i < source.Length && i < target.Length; i++)
            {
                if (source[i] == target[i] || ignoreCase && char.ToLower(source[i]) == char.ToLower(target[i]))
                {
                    diffStartIndex++;
                    continue;
                }
                break;
            }
            if (diffStartIndex > startIndex)
            {
                diff = returnDiffInSource ?
                    source.Substring(diffStartIndex, source.Length - diffStartIndex) :
                    target.Substring(diffStartIndex, target.Length - diffStartIndex);
            }
            else // all different in two strings
            {
                diff = returnDiffInSource ? source : target;
            }
            return diff;
        }

        public static String GetPhoneNumber(this String phoneNumber, bool format = false)
        {
            if (String.IsNullOrEmpty(phoneNumber)) return String.Empty;

            Regex regexNumber = defaultRegexPhoneNumber;

            string number = regexNumber.Replace(phoneNumber, "").Trim(new char[] { ' ', ',' });
            string n_main = String.Empty;
            string n_part = String.Empty;
            string n_hook = String.Empty;

            if (number.Length > 0)
            {
                int x = -1, y = number.Length, z = -1;

                for(int n = 0; n < number.Length; n++)
                {
                    if (z == -1 && number[n] != '!' && number[n] != ',') z = n;
                    if (z != -1 && x == -1 && Char.IsLetter(number[n]) == false) x = n;
                    if (z != -1 && x != -1 && Char.IsLetter(number[n]))
                    {
                        y = n; break;
                    }
                }
                if (z > 0)
                {
                    n_hook = number.Substring(0, z);
                }
                if (x >= 0)
                {
                    n_main = number.Substring(x, y - x);
                    n_part = number.Substring(y, number.Length - y);
                    number = number.Substring(x, number.Length - x);
                }
                else // invalid phone number
                {
                    number = String.Empty;
                }
            }
            if (format)
            {
                number = n_main.GetPhoneNumberWithoutExtension(true) + (n_part.Length > 0 ? (" " + n_part) : "");
            }
            if (n_hook.Length > 0)
            {
                number = (n_hook.Length > 1 ? n_hook : "!,,,") + number;
            }

            return number;
        }

        public static String GetPhoneNumberWithoutExtension(this String phoneNumber, bool formatOutput = false)
        {
            if (String.IsNullOrEmpty(phoneNumber)) return String.Empty;

            Regex regex = defaultRegexPhoneNumber;

            string reg_number = regex.Replace(phoneNumber, "").Trim(new char[] { ' ', ',' }) + "x";
            string raw_number = String.Empty;
            string hook_flash = String.Empty;

            for(int n = 0, x = -1, z = -1; n < reg_number.Length; n++)
            {
                if (z == -1 && reg_number[n] != '!' && reg_number[n] != ',') z = n;
                if (z != -1 && x == -1 && !Char.IsLetter(reg_number[n]))
                {
                    if (z > 0)
                    {
                        hook_flash = (z > 1) ? reg_number.Substring(0, z) : "!,,,";
                    }
                    x = n;
                }
                if (z != -1 && x != -1 && (Char.IsLetter(reg_number[n]) || reg_number[n] == ','))
                {
                    raw_number = reg_number.Substring(x, n - x);
                    break;
                }
            }
            if (formatOutput && !String.IsNullOrEmpty(raw_number) && raw_number.Length > 6)
            {
                if (raw_number.Length == 10)
                {
                    raw_number = "(" + raw_number.Substring(0, 3) + ") " + raw_number.Substring(3, 3) + "-" + raw_number.Substring(6, 4);
                    return hook_flash + raw_number;
                }

                char[] formatted = new char[raw_number.Length + (raw_number.Length - 2) / 3];

                for(int i = 0, j = 0; i < raw_number.Length && j < formatted.Length; i++, j++)
                {
                    int x = raw_number.Length - i - 1;

                    if (i != 0 && x > 0 && x % 3 == 0)
                    {
                        formatted[j] = '-'; j++;
                    }
                    formatted[j] = raw_number[i];
                }
                return hook_flash + new String(formatted);
            }
            return hook_flash + raw_number;
        }

        /// <summary>
        /// Detect if a string is a substring of a full string
        /// </summary>
        /// <param name="ss">substring to be tested</param>
        /// <param name="source">full string</param>
        /// <returns>if it is a substring</returns>
        public static bool IsSubstring(this String ss, String source)
        {
            if (ss == null || source == null || ss.Length == 0 || ss.Length > source.Length) return false;

            int x = 0;

            for(int i = 0; i < source.Length && x < ss.Length; i++)
            {
                int j = i + 1, y = x + 1;
                var isUnicode1 = j < source.Length && Char.IsSurrogatePair(source[i], source[j]);
                var isUnicode2 = y < ss.Length && Char.IsSurrogatePair(ss[x], ss[y]);
                var isSame = false;

                if (isUnicode1 == isUnicode2)
                {
                    isSame = ss[x] == source[i] && (!isUnicode2 || ss[y] == source[j]);
                }
                if (isSame)
                {
                    x += isUnicode1 ? 2 : 1; // move on index of the substring
                }
                else
                {
                    x = 0;
                }

                if (isUnicode1) i++;
            }

            return x == ss.Length;
        }

        /// <summary>
        /// This method will check a url to see that it does not return server or protocol errors
        /// </summary>
        /// <param name="url">The URL path to check</param>
        /// <returns></returns>
        public static bool IsUrlAccessible(this String url)
        {
            if (String.IsNullOrEmpty(url)) return false;

            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD"; //get only the header information - no need to download any content
                request.Timeout = 5000; //keep the user from waiting too long for the page to load

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) //good requests
                {
                    return true;
                }
                else if (statusCode >= 500 && statusCode <= 510) //server errors
                {
                    //the remote server has thrown an internal error. Url is not valid
                    return false;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //error 400
                {
                    return false;
                }
                else
                {
                    //Unhandled status ex.Status
                }
            }
            catch (Exception ex)
            {
                //could not test url
                Debug.WriteLine(ex);
            }
            return false;
        }

        public static bool IsUrlValid(this String url, bool httpOnly = false)
        {
            bool isValid = false;
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "HEAD";
            webRequest.Timeout = 5000;
            try
            {
                using (WebResponse response = webRequest.GetResponse())
                {
                    if (httpOnly == false)
                    {
                        return response.Headers != null;
                    }
                    HttpWebResponse httpResponse = response as HttpWebResponse;

                    if (httpResponse == null && httpOnly)
                    {
                        return false; // Not an HTTP or HTTPS request
                    }

                    int code = (int)httpResponse.StatusCode;

                    //return httpResponse.StatusCode == HttpStatusCode.OK;
                    isValid = (code >= 100 && code <= 510);
                }
            }
            catch //couldn't get response from URL
            {
                isValid = false;
            }
            return isValid;
        }

        #region String - Reverse Methods
        /// <summary>
        /// Reverse a string char by char
        /// </summary>
        /// <param name="s">input string</param>
        /// <returns></returns>
        /// <see cref="http://rosettacode.org/wiki/Reverse_a_string">
        /// </see>
        public static String Reverse(this String s, bool checkUnicode = false)
        {
            if (String.IsNullOrEmpty(s)) return s;

            if (checkUnicode)
            {
                char[] a = new char[s.Length];

                for(int x = 0, j = s.Length - 1; x < s.Length && j >= 0; x++, j--)
                {
                    int i = j - 1, y = x + 1;
                    if (j > 0 && y < s.Length && //&& Char.IsSurrogatePair(s[i], s[j])
                        s[i] >= 0xD800 && s[i] <= 0xDBFF && // Char.IsHighSurrogate(s[i])
                        s[j] >= 0xDC00 && s[j] <= 0xDFFF)   // Char.IsLowSurrogate(s[j])
                    {
                        a[x] = s[i];
                        a[y] = s[j];
                        x++;
                        j--;
                    }
                    else
                    {
                        a[x] = s[j];
                    }
                }
                return new string(a);
            }
            else // 16-bit char
            {
                /*//----Solution 1a: Array
                char[] a = s.ToCharArray(); Array.Reverse(a);
                return new string(a);
                //*/
                /*//----Solution 1b: LINQ
                return new string(s.ToCharArray().Reverse().ToArray());
                //*/
                /*//----Solution 2: StringBuilder*/
                StringBuilder sb = new StringBuilder(s);

                for(int i = 0; i < s.Length / 2; i++)
                {
                    int n = s.Length - i - 1;
                    sb[i] = s[n];
                    sb[n] = s[i];
                }

                //sb[s.Length/2] = s[s.Length/2];
                return sb.ToString();
                //*/
            }
        }
        public static String ReverseByXOR(this String s)
        {
            if (String.IsNullOrEmpty(s)) return s;

            /*//----Solution 2: XOR*/
            ////Question: does this work for unicode?
            char[] a = s.ToCharArray();
            for(int i = 0; i < s.Length / 2; i++)
            {
                int n = s.Length - i - 1;
                a[i] ^= a[n];
                a[n] ^= a[i];
                a[i] ^= a[n];
            }
            return new string(a);
            //*/
        }

        /// <summary>
        /// Reverse a string word by word, optionally considering punctuation
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <see cref="http://rosettacode.org/wiki/Tokenize_a_string">
        /// </see>
        public static String ReverseWord(this String s, bool checkPunctuation = false)
        {
            if (String.IsNullOrEmpty(s)) return s;

            string[] sa = s.Split();

            string rs = String.Join(" ", sa.Reverse());

            return rs;
        }

        #endregion

        #endregion
        
    }

    public class StringToken
    {
        #region Static Members

        public static String[] OpeningTokens =
            {
                "(", "[", "{", "<", 
                "'", "\"", "‘", "“", "«",
                "（", "［", "｛", "〈", "《", "「", "『", "【", "〖", 
                "/*", 
            };
        public static String[] ClosingTokens =
            {
                ")", "]", "}", ">", 
                "'", "\"", "’", "”", "»", 
                "）", "］", "｝", "〉", "》", "」", "』", "】", "〗", 
                "*/", 
            };

        public static bool IsClosingToken(String s)
        {
            if (String.IsNullOrEmpty(s)) return false;

            foreach(var t in ClosingTokens)
            {
                if (s.Length == t.Length && s == t) return true;
            }

            return false;
        }

        public static bool IsOpeningToken(String s)
        {
            if (String.IsNullOrEmpty(s)) return false;

            foreach(var t in OpeningTokens)
            {
                if (s.Length == t.Length && s == t) return true;
            }

            return false;
        }

        #endregion

        /// <summary>
        /// StringToken represents a token in a sentence, 
        /// e.g. a punctuation, a word or a symbol (seperated by blank space or punctuation).
        /// For a punctuation token, it can be defined as "Opening" or "Closing" token
        /// and in such case, ContraPartyToken can be defined.
        /// </summary>
        public StringToken(string content, bool isSymbolToken = false, bool? isPairingToken = null)
        {
            _content = content ?? String.Empty;

            _isSymSign = isSymbolToken;

            if (isPairingToken == false)
            {
                _isClosing = true;
            }
            if (isPairingToken == true)
            {
                _isOpening = true;
            }
        }

        #region Fields
        private string _content = String.Empty;
        private string _contraPartyToken = String.Empty;

        private bool _isClosing;
        private bool _isOpening;
        private bool _isSymSign;

        #endregion

        #region Properties

        public String Content { get { return _content; } }

        public String ContraPartyToken { get { return _contraPartyToken; } }

        public bool IsClosing { get { return _isClosing; } }
        public bool IsNotWord { get { return _isClosing || _isOpening || _isSymSign; } }
        public bool IsOpening { get { return _isOpening; } }
        public bool IsPairedToken { get { return _isClosing || _isOpening; } }
        public bool IsSymbolSign { get { return _isSymSign; } }

        #endregion

    }

}
