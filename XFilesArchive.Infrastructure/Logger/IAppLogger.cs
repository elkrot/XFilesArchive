using System.Diagnostics;

namespace XFilesArchive.Infrastructure
{
    public interface IAppLogger
    {
        void SetLog(string msg, EventLogEntryType eventLogEntryType);
    }
}