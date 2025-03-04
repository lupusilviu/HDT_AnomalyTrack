using System;
using System.IO;

namespace HDT_BGFightTracker
{
    internal class LogsHelper
    {
        public static void WriteToFile(string text)
        {
            string logLine = string.Format("{0}: {1}", DateTime.Now.ToString("HH:mm:ss.fff"), text);
            File.AppendAllLines("C:\\Test\\Logs\\mylog.txt", new string[] { logLine });
        }
    }
}
