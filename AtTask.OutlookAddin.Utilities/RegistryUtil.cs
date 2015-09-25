using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;

namespace AtTask.OutlookAddIn.Utilities
{
    public enum InstallationContexts { NotInstalled, Everyone, JustMe };

    public static class RegistryUtil
    {
        public static string Read(RegistryKey baseRegistryKey, string subKeyName, string keyName)
        {
            //if it's null then (Default) value is needed
            if (keyName != null)
            {
                keyName = keyName.ToUpper();
            }

            RegistryKey subKey = baseRegistryKey.OpenSubKey(subKeyName);
            // If the RegistrySubKey doesn't exist -> (null)
            if (subKey == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return subKey.GetValue(keyName).ToString();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// The method will return the first non-null subkey value by name keyName in the list of subKeyNames
        /// </summary>
        /// <param name="baseRegistryKey"></param>
        /// <param name="subKeyNames"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string Read(RegistryKey baseRegistryKey, string[] subKeyNames, string keyName)
        {
            foreach (var subKeyName in subKeyNames)
            {
                var subKeyValue = Read(baseRegistryKey, subKeyName, keyName);
                if (subKeyValue != null)
                {
                    return subKeyValue;
                }
            }

            return null;
        }

        public static bool Write(RegistryKey baseRegistryKey, string subkeyName, string keyName, object Value)
        {
            try
            {
                RegistryKey sk1 = baseRegistryKey.CreateSubKey(subkeyName);
                // Save the value
                sk1.SetValue(keyName.ToUpper(), Value);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteKey(RegistryKey baseRegistryKey, string subKey, string keyName)
        {
            try
            {
                RegistryKey sk1 = baseRegistryKey.CreateSubKey(subKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                {
                    return true;
                }
                else
                {
                    sk1.DeleteValue(keyName);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteSubKeyTree(RegistryKey baseRegistryKey, string subKeyName)
        {
            try
            {
                RegistryKey subKey = baseRegistryKey.OpenSubKey(subKeyName);
                // If the RegistryKey exists, I delete it
                if (subKey != null)
                {
                    baseRegistryKey.DeleteSubKeyTree(subKeyName);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static InstallationContexts GetInstalledContext(params string[] appNames)
        {
            foreach (string appName in appNames)
            {
                InstallationContexts appContext = GetInstalledContext(appName);
                if (appContext != InstallationContexts.NotInstalled)
                {
                    return appContext;
                }
            }

            return InstallationContexts.NotInstalled;
        }

        public static InstallationContexts GetInstalledContext(string appDisplayName)
        {
            //The S-1-5-18  =
            //Local System user (Everyone) | HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products\

            //The S-XXXXXX  =
            //Current user (Just me)       |
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-?!?!?!?!\Products\

            string key;
            string keyFormat = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\{0}\Products\";

            // Check if installed for -> Everyone
            key = string.Format(keyFormat, "S-1-5-18");
            bool res = IsAddinInstalled(key, appDisplayName, StringComparison.OrdinalIgnoreCase);
            if (res == true)
            {
                return InstallationContexts.Everyone;
            }

            // Check if installed for -> Just me
            key = string.Format(keyFormat, WindowsIdentity.GetCurrent().User.Value);
            res = IsAddinInstalled(key, appDisplayName, StringComparison.OrdinalIgnoreCase);
            if (res == true)
            {
                return InstallationContexts.JustMe;
            }

            return InstallationContexts.NotInstalled;
        }

        public static RegistryKey GetRegistryKey()
        {
            return GetRegistryKey(null);
        }

        public static RegistryKey GetRegistryKey(string keyPath, RegistryHive regHive = RegistryHive.LocalMachine)
        {
            RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(regHive, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
            return string.IsNullOrEmpty(keyPath) ? localMachineRegistry : localMachineRegistry.OpenSubKey(keyPath);
        }

        public static object GetRegistryValue(string keyPath, string keyName)
        {
            RegistryKey registry = GetRegistryKey(keyPath);
            return registry.GetValue(keyName);
        }

        private static bool IsAddinInstalled(string regKey, string appDisplayName, StringComparison compare)
        {
            using (RegistryKey regkey = GetRegistryKey(regKey))
            {
                if (regkey != null)
                {
                    RegistryKey rk;
                    string[] arrProducs = regkey.GetSubKeyNames();
                    for (int i = 0; i < arrProducs.Length; i++)
                    {
                        using (rk = regkey.OpenSubKey(arrProducs[i] + @"\InstallProperties"))
                        {
                            if (rk == null)
                            {
                                continue;
                            }

                            object displayName = rk.GetValue("DisplayName");
                            if (displayName == null)
                            {
                                continue;
                            }

                            if (appDisplayName.Equals(displayName.ToString(), compare) == true)
                            {
                                return true;
                            }

                            if (appDisplayName.StartsWith(displayName.ToString(), compare)
                                || displayName.ToString().StartsWith(appDisplayName, compare))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static List<KeyValuePair<string, string>> GetOutlookAddins()
        {
            List<KeyValuePair<string, RegistryHive>> regKeys = new List<KeyValuePair<string, RegistryHive>>
            {
                new KeyValuePair<string, RegistryHive>(@"SOFTWARE\Wow6432Node\Microsoft\Office\Outlook\Addins", RegistryHive.LocalMachine),
                new KeyValuePair<string, RegistryHive>(@"SOFTWARE\Microsoft\Office\Outlook\Addins", RegistryHive.LocalMachine),
                new KeyValuePair<string, RegistryHive>(@"SOFTWARE\Microsoft\Office\Outlook\Addins", RegistryHive.CurrentUser),
                new KeyValuePair<string, RegistryHive>(string.Format(@"{0}\Software\Microsoft\Office\Outlook\Addins", WindowsIdentity.GetCurrent().User.Value), RegistryHive.Users)
            };

            List<KeyValuePair<string, string>> addins = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < regKeys.Count; i++)
            {
                RegistryKey regKey = RegistryUtil.GetRegistryKey(regKeys[i].Key, regKeys[i].Value);
                if (regKey != null)
                {
                    regKey.GetSubKeyNames().ToList<string>().ForEach((string name) =>
                    {
                        RegistryKey subKey = regKey.OpenSubKey(name);
                        string friendlyName = subKey.GetValue("FriendlyName") as string;

                        if (!addins.Any(x => x.Key == name && x.Value == friendlyName))
                        {
                            addins.Add(new KeyValuePair<string, string>(name, friendlyName));
                        }
                    });
                }
            }

            return addins;
        }

    }
}