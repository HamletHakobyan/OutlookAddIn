using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class EmailUtil
    {
        private static readonly Regex EmailAddressesRegex = new Regex(@"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                            + @"([a-zA-Z]+[\w-]?\.)+[a-zA-Z]{1,10})", RegexOptions.Compiled);

        //used to extract e.g. TASK_4eb387010001cdc87b0d5983eac5962a from
        //"Open an email item directly in Outlook" - New Outlook Team Request [TASK_4eb387010001cdc87b0d5983eac5962a]
        private static readonly Regex AtTaskEmailSubjectRegex = new Regex(@".*\[([a-zA-Z]+_[0-9a-f]{32})\]$", RegexOptions.Compiled);

        /// <summary>
        /// Strips all re:-s fw:-s and fwd:-s from the subject beginning
        /// </summary>
        /// <param name="subject"></param>
        public static void MailSubjectToTaskName(ref string subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return;
            }

            subject = Regex.Replace(subject.Trim(), "^(re: |fw: |fwd: )*", "", RegexOptions.IgnoreCase).Trim();
        }

        public static bool TryGetEmailAddresses(string emailString, out List<string> emailAddresses)
        {
            // Find matches.
            MatchCollection matches = EmailAddressesRegex.Matches(emailString);

            if (matches.Count > 0)
            {
                emailAddresses = new List<string>();

                foreach (Match match in matches)
                {
                    emailAddresses.Add(match.Value.ToString());
                }

                return true;
            }

            emailAddresses = null;
            return false;
        }

        /// <summary>
        /// Parses the given string assuming that it's a subject of notification e-mail from AtTask.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="id"></param>
        /// <param name="objCode"></param>
        /// <returns></returns>
        public static bool ParseAtTaskEmailSubject(string subject, out string id, out string objCode)
        {
            if (!string.IsNullOrWhiteSpace(subject))
            {
                Match match = AtTaskEmailSubjectRegex.Match(subject);
                if (match.Groups.Count == 2)
                {
                    string str = match.Groups[1].Value;
                    string[] values = str.Split('_');
                    id = values[1];
                    objCode = values[0];
                    return true;
                }
            }

            id = null;
            objCode = null;
            return false;
        }
    }
}
