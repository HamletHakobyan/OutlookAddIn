using System.Diagnostics;

namespace AtTask.OutlookAddIn.Utilities
{
    public class ProcessUtil
    {
        public static bool FindAndKillProcess(string name)
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes by using the StartsWith Method,
                //this prevents us from incluing the .EXE for the process we're looking for.
                //. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running
                if (clsProcess.ProcessName.StartsWith(name))
                {
                    //since we found the proccess we now need to use the
                    //Kill Method to kill the process. Remember, if you have
                    //the process running more than once, say IE open 4
                    //times the loop thr way it is now will close all 4,
                    //if you want it to just close the first one it finds
                    //then add a return; after the Kill
                    clsProcess.Kill();
                    //process killed, return true
                    return true;
                }
            }
            //process not found, return false
            return false;
        }

        /// <summary>
        /// Checks if program with specified name is running.
        /// </summary>
        /// <param name="name">Program Name e.g. "OUTLOOK"</param>
        /// <returns>if no instance of program is running method return false otherwise true.</returns>
        public static bool IsProgramRunning(string name)
        {
            Process[] pName = Process.GetProcessesByName(name);
            if (pName.Length == 0)
            {
                //Program is not running.
                return false;
            }
            else
            {
                //Program is running.
                return true;
            }
        }
    }
}