using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Common.Extensions
{
    public static class SecureStringExtensions
    {
        public static SecureString ConvertToSecureString(this string strPassword)
        {
            if (string.IsNullOrEmpty(strPassword))
                return null;

            var secureStr = new SecureString();
            if (!string.IsNullOrEmpty(strPassword))
            {
                foreach (var c in strPassword.ToCharArray()) 
                    secureStr.AppendChar(c);
            }
            return secureStr;

            /*
            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    var securePassword = new SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }              
            */
        }

        public static string ConvertToUnsecureString(this SecureString secureStr)
        {
            if (secureStr == null)
                return null;

            IntPtr unmanagedStr = IntPtr.Zero;
            try
            {
                unmanagedStr = Marshal.SecureStringToGlobalAllocUnicode(secureStr);
                return Marshal.PtrToStringUni(unmanagedStr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedStr);
            }
        }
    }
}
