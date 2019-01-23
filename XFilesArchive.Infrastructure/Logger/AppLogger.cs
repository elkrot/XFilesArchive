using System.Diagnostics;


namespace XFilesArchive.Infrastructure
{
    public class AppLogger : IAppLogger
    {
        string _logName ;
        string _eventLogSource;

        public AppLogger(string eventLogSource="XFileArchive", string logName = "Application")
        {
            _eventLogSource = eventLogSource;
            _logName = logName;
        }


        public void SetLog(string msg, EventLogEntryType eventLogEntryType) {
            using (EventLog eventLog = new EventLog(_logName))
            {
                eventLog.Source = _eventLogSource;
                eventLog.WriteEntry(msg, eventLogEntryType);
            }
        }
    }
}
