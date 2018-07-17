﻿using HomeArchiveX.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace HomeArchiveX.Infrastructure
{
    public interface IFIleManager
    {
        Image ResizeImg(Image b, int nWidth, int nHeight);
        string CopyImg(string imgPath, string targetDir);
        Bitmap GetThumb(string imgPath);
        string SaveThumb(string targetRootDir, string thumbDir, Bitmap bmp, string thumbName);
        bool IsImage(string ext);
        MethodResult<int> FillDirectoriesInfo(int driveId, string pathDrive, Func<string, int, int> CreateFolder);
        MethodResult<int> FillFilesInfo(int driveId, string pathDrive, Func<string, int, int> CreateFile);
        DirectoryInfo GetDirectoryInfoByPath(string path);
        FileInfo GetFileInfoByPath(string path);
        DriveX FillDrive(DriveX drive, byte[] bytes, string title, int id);
        byte[] GetBinaryData<T>(T obj);
        byte[] GetImageData(Bitmap bmp);
        T GetDataFromBinary<T>(byte[] data);

    }
}