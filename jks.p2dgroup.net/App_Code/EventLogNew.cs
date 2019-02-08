using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Summary description for EventLog
/// </summary>
public class EventLogNew
{
	
		public static void Write(string ApplicationName, string LogText, System.Diagnostics.EventLogEntryType EventLogType, int EventID)
          {
            //StringBuilder logPath = new StringBuilder(Environment.CurrentDirectory + "\\Logs");
              StringBuilder logPath = new StringBuilder(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Logs");
            if (!Directory.Exists(logPath.ToString()))
            {
                Directory.CreateDirectory(logPath.ToString());
            }

            string fileName = "Log-" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";

            logPath.Append("\\" + fileName);

            if (!File.Exists(logPath.ToString()))
            {
                FileStream f = File.Create(logPath.ToString());
                f.Close();
            }

            StreamWriter sw = new StreamWriter(logPath.ToString(), true);
            sw.WriteLine(DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + ": " + LogText + ", EventID: " + EventID + ", EventLogType: " + EventLogType.ToString());
            sw.Flush();
            sw.Close();

            EventLog.WriteEntry(ApplicationName, LogText, EventLogType, EventID);

        }
	
}