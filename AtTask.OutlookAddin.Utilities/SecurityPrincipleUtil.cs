using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace AtTask.OutlookAddIn.Utilities
{
    public enum SecurityProductsCategory
    {
        AntiVirusProduct,
        AntiSpywareProduct,
        FirewallProduct
    }

    public class SecurityPrincipleUtil
    {
        [DllImport("kernel32")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32")]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, Int32 DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("advapi32")]
        private static extern bool GetTokenInformation(IntPtr hToken, UInt32 tokenInfoClass, IntPtr TokenInformation,
                                               Int32 tokeInfoLength, ref Int32 reqLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetTokenInformation(IntPtr tokenHandle, TokenInformationClass tokenInformationClass, IntPtr tokenInformation, int tokenInformationLength, out int returnLength);

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private extern static bool InternetGetConnectedState(ref InternetConnectionState_e lpdwFlags, int dwReserved);

        
        [DllImport("advapi32")]
        private static extern bool IsWellKnownSid(IntPtr pSid, Int32 sidType);

        [StructLayout(LayoutKind.Sequential)]
        public struct _SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public Int32 Attributes;
        }

        /// <summary>
        /// Passed to <see cref="GetTokenInformation"/> to specify what
        /// information about the token to return.
        /// </summary>
        enum TokenInformationClass:uint
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUiAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        /// <summary>
        /// The elevation type for a user token.
        /// </summary>
        enum TokenElevationType
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }


        // Returns true if the user that started the process is a member of the admin group
        // (which does not necessarily mean that the process has admin privileges)
        public static bool IsAdmin()
        {
            bool isAdmin = false;
            IntPtr token = IntPtr.Zero;
            OpenProcessToken(GetCurrentProcess(), 0x0008, ref token); // TOKEN_QUERY = 0x0008
            Int32 len = 0;
            GetTokenInformation(token, 2, IntPtr.Zero, 0, ref len); // TOKEN_GROUPS = 2
            IntPtr ti = Marshal.AllocHGlobal(len);
            GetTokenInformation(token, 2, ti, len, ref len);
            int nGroups = Marshal.ReadInt32(ti);
            UInt64 pSaa = (UInt64)ti + (UInt64)IntPtr.Size;

            for (int i = 0; i < nGroups; i++)
            {
                _SID_AND_ATTRIBUTES saa = (_SID_AND_ATTRIBUTES)Marshal.PtrToStructure((IntPtr)pSaa, typeof(_SID_AND_ATTRIBUTES));
                if (IsWellKnownSid(saa.Sid, 26))
                {
                    isAdmin = true; // WinBuiltinAdministratorsSid = 26
                    break;
                }
                pSaa += (UInt32)Marshal.SizeOf(typeof(_SID_AND_ATTRIBUTES));
            }
            Marshal.FreeHGlobal(ti);

            return isAdmin;
        }

        public static void GetRole()
        {
            System.AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            WindowsIdentity curIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal myPrincipal = new WindowsPrincipal(curIdentity);

            List<string> groups = new List<string>();

            foreach (IdentityReference irc in curIdentity.Groups)
            {
                groups.Add(((NTAccount)irc.Translate(typeof(NTAccount))).Value);
            }

            Console.WriteLine(
                           @"Name:          {0},
                           System:          {1}
                           Authenticated:   {2}
                           BuiltinAdmin:    {3}
                           Identity:        {4}
                           Groups:          {5}",
                curIdentity.Name,
                curIdentity.IsSystem,
                curIdentity.IsAuthenticated,
                myPrincipal.IsInRole(WindowsBuiltInRole.Administrator) ? "True" : "False",
                myPrincipal.Identity,
                string.Join(string.Format(",{0}\t\t", Environment.NewLine), groups.ToArray()));

            try
            {
                Console.WriteLine(Environment.NewLine);
            }
            catch (System.Security.SecurityException scx)
            {
                Console.WriteLine(scx.Message + " " + scx.FirstPermissionThatFailed.ToString());
            }

            Console.WriteLine(Environment.NewLine);
        }

        public static bool IsAdministratorInElevatedProcess()
        {
            WindowsIdentity curIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal myPrincipal = new WindowsPrincipal(curIdentity);

            return myPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static string GetInstalledSecurityProducts(string category)
        {
            try
            {
                ConnectionOptions _connectionOptions = new ConnectionOptions();
                //Not required while checking it in local machine. 
                //For remote machines you need to provide the credentials
                //options.Username = "";
                //options.Password = "";
                _connectionOptions.EnablePrivileges = true;
                _connectionOptions.Impersonation = ImpersonationLevel.Impersonate;
                //Connecting to SecurityCenter2 node for querying security details

                ManagementScope _managementScope = null;
                //Windows Vista/7/8
                if (Environment.OSVersion.Version.Major > 5)
                {
                    _managementScope = new ManagementScope(string.Format("\\\\{0}\\root\\SecurityCenter2", Environment.MachineName), _connectionOptions);
                }
                else
                {
                    _managementScope = new ManagementScope(string.Format("\\\\{0}\\root\\SecurityCenter", Environment.MachineName), _connectionOptions);
                }

                _managementScope.Connect();
                //Querying
                ObjectQuery _objectQuery = new ObjectQuery("SELECT * FROM " + category); //AntiVirusProduct, AntiSpywareProduct, FirewallProduct
                using(ManagementObjectSearcher _managementObjectSearcher = new ManagementObjectSearcher(_managementScope, _objectQuery))
                using(ManagementObjectCollection _managementObjectCollection = _managementObjectSearcher.Get())
                {
                    if (_managementObjectCollection.Count > 0)
                    {
                        List<string> products = new List<string>();
                        foreach (ManagementObject item in _managementObjectCollection)
                        {
                            if (item["displayName"] != null)
                            {
                                products.Add(item["displayName"].ToString());
                            }
                        }

                        return string.Join(", ", products);
                    }
                    
                }
            }
            catch
            {
                return "None";
            }
            return "None";
        }
        
        public static string UserRoles()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            List<WindowsBuiltInRole> roles = new List<WindowsBuiltInRole>();
            foreach (WindowsBuiltInRole role in Enum.GetValues(typeof(WindowsBuiltInRole)))
            {
                if (principal.IsInRole(role))
                {
                    roles.Add(role);
                }
            }

            return string.Join(", ", roles);
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            if (identity == null) throw new InvalidOperationException("Couldn't get the current user identity");
            var principal = new WindowsPrincipal(identity);

            // Check if this user has the Administrator role. If they do, return immediately.
            // If UAC is on, and the process is not elevated, then this will actually return false.
            if (principal.IsInRole(WindowsBuiltInRole.Administrator)) return true;

            // If we're not running in Vista onwards, we don't have to worry about checking for UAC.
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 6)
            {
                // Operating system does not support UAC; skipping elevation check.
                return false;
            }

            int tokenInfLength = Marshal.SizeOf(typeof(int));
            IntPtr tokenInformation = Marshal.AllocHGlobal(tokenInfLength);

            try
            {
                var token = identity.Token;
                var result = GetTokenInformation(token, (uint)TokenInformationClass.TokenElevationType, tokenInformation, tokenInfLength, ref tokenInfLength);

                if (!result)
                {
                    var exception = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                    throw new InvalidOperationException("Couldn't get token information", exception);
                }

                var elevationType = (TokenElevationType)Marshal.ReadInt32(tokenInformation);

                switch (elevationType)
                {
                    case TokenElevationType.TokenElevationTypeDefault:
                        // TokenElevationTypeDefault - User is not using a split token, so they cannot elevate.
                        return false;
                    case TokenElevationType.TokenElevationTypeFull:
                        // TokenElevationTypeFull - User has a split token, and the process is running elevated. Assuming they're an administrator.
                        return true;
                    case TokenElevationType.TokenElevationTypeLimited:
                        // TokenElevationTypeLimited - User has a split token, but the process is not running elevated. Assuming they're an administrator.
                        return true;
                    default:
                        // Unknown token elevation type.
                        return false;
                }
            }
            finally
            {
                if (tokenInformation != IntPtr.Zero) Marshal.FreeHGlobal(tokenInformation);
            }
        }

        [Flags]
        enum InternetConnectionState_e : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        // Return true or false if connecting through a proxy server
        public static bool IsProxyEnabled()
        {
            InternetConnectionState_e flags = 0;
            InternetGetConnectedState(ref flags, 0);
            bool hasProxy = false;

            if ((flags & InternetConnectionState_e.INTERNET_CONNECTION_PROXY) != 0)
            {
                hasProxy = true;
            }
            else
            {
                hasProxy = false;
            }

            return hasProxy;
        }


        /// <summary>
        /// The function checks whether the primary access token of the process belongs
        /// to user account that is a member of the local Administrators group, even if
        /// it currently is not elevated.
        /// </summary>
        /// <returns>
        /// Returns true if the primary access token of the process belongs to user
        /// account that is a member of the local Administrators group. Returns false
        /// if the token does not.
        /// </returns>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// When any native Windows API call fails, the function throws a Win32Exception
        /// with the last error code.
        /// </exception>
        //internal bool IsUserInAdminGroup()
        //{
        //    bool fInAdminGroup = false;
        //    SafeTokenHandle hToken = null;
        //    SafeTokenHandle hTokenToCheck = null;
        //    IntPtr pElevationType = IntPtr.Zero;
        //    IntPtr pLinkedToken = IntPtr.Zero;
        //    int cbSize = 0;

        //    try
        //    {
        //        // Open the access token of the current process for query and duplicate.
        //        if (!NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle,
        //            NativeMethods.TOKEN_QUERY | NativeMethods.TOKEN_DUPLICATE, out hToken))
        //        {
        //            throw new Win32Exception();
        //        }

        //        // Determine whether system is running Windows Vista or later operating
        //        // systems (major version >= 6) because they support linked tokens, but
        //        // previous versions (major version < 6) do not.
        //        if (Environment.OSVersion.Version.Major >= 6)
        //        {
        //            // Running Windows Vista or later (major version >= 6).
        //            // Determine token type: limited, elevated, or default.

        //            // Allocate a buffer for the elevation type information.
        //            cbSize = sizeof(TOKEN_ELEVATION_TYPE);
        //            pElevationType = Marshal.AllocHGlobal(cbSize);
        //            if (pElevationType == IntPtr.Zero)
        //            {
        //                throw new Win32Exception();
        //            }

        //            // Retrieve token elevation type information.
        //            if (!NativeMethods.GetTokenInformation(hToken,
        //                TOKEN_INFORMATION_CLASS.TokenElevationType, pElevationType,
        //                cbSize, out cbSize))
        //            {
        //                throw new Win32Exception();
        //            }

        //            // Marshal the TOKEN_ELEVATION_TYPE enum from native to .NET.
        //            TOKEN_ELEVATION_TYPE elevType = (TOKEN_ELEVATION_TYPE)
        //                Marshal.ReadInt32(pElevationType);

        //            // If limited, get the linked elevated token for further check.
        //            if (elevType == TOKEN_ELEVATION_TYPE.TokenElevationTypeLimited)
        //            {
        //                // Allocate a buffer for the linked token.
        //                cbSize = IntPtr.Size;
        //                pLinkedToken = Marshal.AllocHGlobal(cbSize);
        //                if (pLinkedToken == IntPtr.Zero)
        //                {
        //                    throw new Win32Exception();
        //                }

        //                // Get the linked token.
        //                if (!NativeMethods.GetTokenInformation(hToken,
        //                    TOKEN_INFORMATION_CLASS.TokenLinkedToken, pLinkedToken,
        //                    cbSize, out cbSize))
        //                {
        //                    throw new Win32Exception();
        //                }

        //                // Marshal the linked token value from native to .NET.
        //                IntPtr hLinkedToken = Marshal.ReadIntPtr(pLinkedToken);
        //                hTokenToCheck = new SafeTokenHandle(hLinkedToken);
        //            }
        //        }

        //        // CheckTokenMembership requires an impersonation token. If we just got
        //        // a linked token, it already is an impersonation token.  If we did not
        //        // get a linked token, duplicate the original into an impersonation
        //        // token for CheckTokenMembership.
        //        if (hTokenToCheck == null)
        //        {
        //            if (!NativeMethods.DuplicateToken(hToken,
        //                SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
        //                out hTokenToCheck))
        //            {
        //                throw new Win32Exception();
        //            }
        //        }

        //        // Check if the token to be checked contains admin SID.
        //        WindowsIdentity id = new WindowsIdentity(hTokenToCheck.DangerousGetHandle());
        //        WindowsPrincipal principal = new WindowsPrincipal(id);
        //        fInAdminGroup = principal.IsInRole(WindowsBuiltInRole.Administrator);
        //    }
        //    finally
        //    {
        //        // Centralized cleanup for all allocated resources.
        //        if (hToken != null)
        //        {
        //            hToken.Close();
        //            hToken = null;
        //        }
        //        if (hTokenToCheck != null)
        //        {
        //            hTokenToCheck.Close();
        //            hTokenToCheck = null;
        //        }
        //        if (pElevationType != IntPtr.Zero)
        //        {
        //            Marshal.FreeHGlobal(pElevationType);
        //            pElevationType = IntPtr.Zero;
        //        }
        //        if (pLinkedToken != IntPtr.Zero)
        //        {
        //            Marshal.FreeHGlobal(pLinkedToken);
        //            pLinkedToken = IntPtr.Zero;
        //        }
        //    }

        //    return fInAdminGroup;
        //}
    }
}