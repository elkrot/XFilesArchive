
using System;
using System.Drawing;
using System.IO;
using XFilesArchive.Infrastructure;

namespace HomeArchiveX.Infrastructure
{
    public interface IFIleManager
    {
        Image ResizeImg(Image b, int nWidth, int nHeight);
        string CopyImg(string imgPath, string targetDir);
        Bitmap GetThumb(string imgPath);
        /// <summary>
        /// Сохранить на диске Эскиз
        /// </summary>
        /// <param name="targetRootDir">Корневая дирректоря программы</param>
        /// <param name="thumbDir">Дирректория с эскизами</param>
        /// <param name="bmp">Изображение</param>
        /// <param name="thumbName">Наименование эскиза</param>
        /// <returns></returns>
        string SaveThumb(string targetRootDir, string thumbDir, Bitmap bmp, string thumbName);
        string SaveThumbTemp(string targetTempDir, string thumbDir, Bitmap bmp, string thumbName);
        bool IsImage(string ext);
        bool IsMedia(string ext);
        MethodResult<int> FillDirectoriesInfo(int driveId, string pathDrive, Func<string, int, int> CreateFolder);
        MethodResult<int> FillFilesInfo(int driveId, string pathDrive, Func<string, int, int> CreateFile);
        DirectoryInfo GetDirectoryInfoByPath(string path);
        FileInfo GetFileInfoByPath(string path);
        DriveX FillDrive(DriveX drive, byte[] bytes, string title, int id);
        byte[] GetBinaryData<T>(T obj);
        byte[] GetImageData(Bitmap bmp);
        T GetDataFromBinary<T>(byte[] data);
        void DeleteDirectory(string tempDirectory);
    }
}