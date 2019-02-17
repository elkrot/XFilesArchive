using System.Collections.Generic;
using System.IO;
using XFilesArchive.Model;

namespace XFilesArchive.Infrastructure.DataManager
{
    public interface IDataManager
    {
        ILogger logger { get; }
        int CreateFolder(string path, int driveId);
        int CreateFile(string path, int driveId);
        void CreateImageToEntity(string ImagePath, int entityId, int driveId);
        int CreateImage(string imagePath, string targetDir);
        DriveX GetDriveById(int id);
        int GetEntityIdByPath(string path, int driveId, EntityType entityType);
        int CreateDrive(string path, string title, string diskCode, Dictionary<string, object> addParams = null);
        int CreateArchiveEntity<T>(int driveId, T entity, string title,
                   int fileSize, int parentEntityKey, EntityType entityType, string entityPath
            , string extension, string description,
                   string checksum);
        Dictionary<string, string> GetFileInfoById(int id);
        DriveInfo GetDriveInfoById(int id);
        Dictionary<string, string> GetDirectoryInfoById(int id);
        int IsDriveExist(int hashCode, string title);
        void FillDirectoriesInfo(int driveId, string pathDrive);
        void FillFilesInfo(int driveId, string pathDrive);
        void TruncateTables();
        void ClearCash();

        string[] GetDrives();
        string[] GetDirectories(int id);
        string[] GetFiles(int id);
        Dictionary<string, string> GetMediaFileInfoById(int id);

        string[] CheckFilesByHashOrTitle(int fileSize, string checksum, string title);

        void SetFileSize(int archiveEntityKey, int fileSize);
        void SetFileSizeByKeys(int driveId);
        void BulkCopyArchiveEntity(string cs, IEnumerable<DestinationItem> items,int DriveId);
        void BulkCopyImage(string cs, IEnumerable<ImageDto> items);
    }
}