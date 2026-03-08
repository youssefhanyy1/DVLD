using DVLD_Business;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fullProject.Global_Classes
{
    public class clsStoreInRegistry
    {

        private static readonly string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\MyDataLogin";
        private const string UserNameName = "UserName";
        private const string PasswordName = "Password";

        public static bool RememberUsernameAndPassword(string username, string password)
        {
            try
            {
                Registry.SetValue(keyPath, UserNameName, username, RegistryValueKind.String);
                Registry.SetValue(keyPath, PasswordName, password, RegistryValueKind.String);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetStoredCredential(out string username, out string password)
        {
            username = Registry.GetValue(keyPath, UserNameName, null) as string;
            password = Registry.GetValue(keyPath, PasswordName, null) as string;

            return !string.IsNullOrEmpty(username) &&
                   !string.IsNullOrEmpty(password);
        }
    }
}