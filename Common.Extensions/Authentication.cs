using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Security;

namespace Common.Extensions
{
    public enum AuthenticationType { None, Windows }

    public class LoginIdentity
    {
        private string _userName = String.Empty;
        private string _displayName = String.Empty;

        public LoginIdentity(string userName, string displayName)
        {
            this._userName = userName;
            this._displayName = displayName;
        }
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        }

        public string DisplayName
        {
            get { return this._displayName; }
            set { this._displayName = value; }
        }
    }

    public interface IAuthentication
    {
        AuthenticationType AutheticationType { get; }

        LoginIdentity GetLoginIdentity();
    }

    public class NoneAuthentication : IAuthentication
    {
        public NoneAuthentication() { }

        public AuthenticationType AutheticationType
        {
            get { return AuthenticationType.None; }
        }

        public LoginIdentity GetLoginIdentity()
        {
            return new LoginIdentity(String.Empty, String.Empty);
        }
    }

    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class SafeNativeMethods
    {
        [DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError=true)]        
        internal static extern int GetUserNameEx(int nameFormat, StringBuilder userName, ref uint userNameSize);
    }

    public class WindowsAuthentication : IAuthentication
    {
        enum EXTENDED_NAME_FORMAT
        {
            NameUnknown = 0,
            NameFullyQualifiedDN = 1,
            NameSamCompatible = 2,
            NameDisplay = 3,
            NameUniqueId = 6,
            NameCanonical = 7,
            NameUserPrincipal = 8,
            NameCanonicalEx = 9,
            NameServicePrincipal = 10,
            NameDnsDomain = 12
        }        
       
        public AuthenticationType AutheticationType
        {
            get { return AuthenticationType.Windows; }
        }

        public LoginIdentity GetLoginIdentity()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            string shortAccountName = identity.Name.Substring(identity.Name.IndexOf("\\") + 1, identity.Name.Length - identity.Name.IndexOf("\\") - 1);
            string fullName = getUserName();

            LoginIdentity loginIdentity = new LoginIdentity(identity.Name, fullName);

            return loginIdentity;
        }

        private string getUserName()
        {
            StringBuilder userName = new StringBuilder(1024);
            uint userNameSize = (uint)userName.Capacity;

            if (0 != SafeNativeMethods.GetUserNameEx((int)EXTENDED_NAME_FORMAT.NameDisplay, userName, ref userNameSize))
            {
                string[] nameParts = userName.ToString().Split('\\');
                return nameParts[0];
            }

            return "";
        }

        public WindowsAuthentication() { }

    }
}
