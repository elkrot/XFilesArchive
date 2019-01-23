namespace XFilesArchive.Infrastructure
{
    public interface ILogger
    {
        void Add(string message);
        string GetLog();
    }
}
