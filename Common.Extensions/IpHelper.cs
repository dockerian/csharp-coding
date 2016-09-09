using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    public static class IpHelper
    {
        public static String GetLocalIpAddress(this IPHostEntry host)
        {
            foreach(IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    return ip.ToString();
                }
            }
            return String.Empty;
        }

        public static String[] GetLocalIpAddressFields(this IPHostEntry host)
        {
            string[] fields = new string[] { "", "", "", "" };
            foreach(IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString().Split(new char[] { '.' });
                }
            }
            return fields;
        }

        public static void GoToURL(string url) 
        {
            /* From: http://support.microsoft.com/kb/305703 */
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    string errorInfo = string.Format("GoToURL error - {0}", noBrowser.Message);
                    System.Diagnostics.Debug.WriteLine(errorInfo);
                }
            }
            catch (System.Exception other)
            {
                string errorInfo = string.Format("GoToURL error - {0}", other.Message);
                System.Diagnostics.Debug.WriteLine(errorInfo);
            }
        }

        public static bool ValidateHostname(string hostName)
        {
            /* pattern is valid as per RFC 952. Later, RFC 1123 introduced a change 
             * stating that hostname segments may also start with a digit.
            /* http://en.wikipedia.org/wiki/Hostname */

            const string pattern = @"^(([a-zA-Z]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$";
            bool bValidHostname = Regex.IsMatch(hostName, pattern);
            string ipAddr = ValidateIpAddress(hostName);
            return (bValidHostname || (ipAddr != null) || (hostName == string.Empty));
        }

        public static string ValidateIpAddress(string ipString)
        {
            /* From http://regexlib.com/REDetails.aspx?regexp_id=2481 */
            /* author - Rafael Henrique Kato Assis */
            string pattern = @"^((0|1[0-9]{0,2}|2[0-9]{0,1}|2[0-4][0-9]|25[0-5]|[3-9][0-9]{0,1})\.){3}(0|1[0-9]{0,2}|2[0-9]{0,1}|2[0-4][0-9]|25[0-5]|[3-9][0-9]{0,1})(?(\/)\/([0-9]|[1-2][0-9]|3[0-2])|)$";
            bool isMatch = Regex.IsMatch(ipString, pattern);

            if (isMatch)
            {
                return ipString;
            }
            IPAddress ipAddress;

            if (IPAddress.TryParse(ipString, out ipAddress))
            {
                return ipAddress.ToString();
            }

            return null;
        }

    }
}
