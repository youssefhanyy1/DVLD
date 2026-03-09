using System;
using System.Diagnostics;

namespace Utils
{
    public class Log
    {
        public static void LogException(Exception ex, string senderName, string senderLocation)
        {
            string message =
                      $@"{Environment.NewLine}
                      -------------------------------------------
                      Date     : {DateTime.Now:dd/MM/yyyy hh:mm:ss} 
                      -------------------------------------------
                      sender   : {senderName}
                      location : {senderLocation}
                      Message  : {ex.Message}

                       {Environment.NewLine}";

            string soureName = "DVLD";

            if (!EventLog.SourceExists(soureName))
            {
                EventLog.CreateEventSource(soureName, "Application");
            }

            EventLog.WriteEntry(soureName, message, EventLogEntryType.Error);

        }

    }
}