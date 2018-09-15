namespace XFilesArchive.Infrastructure
{
    public interface IConfiguration
    {
        int ThumbnailWidth { get;  }
        int ThumbnailHeight { get; }
        string GetConnectionString();
        string GetThumbDirName();
        string GetTargetImagePath();
    }
}