using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class CryptoUtil
    {
        /// <summary>
        /// Creates MD5 hash of given string.
        /// </summary>
        /// <param name="str">String to hash.</param>
        /// <returns>Hashed string.</returns>
        public static string MD5HashString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            byte[] hashBytes;
            using (MD5 md5 = MD5.Create())
            {
                try
                {
                    byte[] buffer = Encoding.Default.GetBytes(str);
                    hashBytes = md5.ComputeHash(buffer);
                }
                catch
                {
                    return string.Empty;
                }
            }

            StringBuilder hashSB = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                //Format each byte of hash data as a hexadecimal string.
                hashSB.Append(hashBytes[i].ToString("x2"));
            }

            return hashSB.ToString();
        }
    }
}
