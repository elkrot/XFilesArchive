﻿using HomeArchiveX.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure.DataManager;
using XFilesArchive.Model;

namespace XFilesArchive.Infrastructure
{
    public class DestinationManager
    {
        private FillInfoParameters _imgInfoParams;
        private IDataManager _dm;
        private IFIleManager _fm;
        private ConfigurationData _cnf;
        const string ERROR_ARGUMENT_EXCEPTION_MSG = "Не верно указан параметр";

        public DestinationManager(FillInfoParameters imgInfoParams
            , IDataManager dm, IFIleManager fm, ConfigurationData cnf)
        {
            _imgInfoParams = imgInfoParams;
            _dm = dm;
            _fm = fm;
            _cnf = cnf;
        }

        #region CreateDestinationList
        public MethodResult<List<DestinationItem>> CreateDestinationList()
        {
            var list = new List<DestinationItem>();
            var result = new MethodResult<List<DestinationItem>>(list);
            try
            {
                FillDestinationItemsList(_imgInfoParams.DestinationPath, ref list);
            }
            catch (UnauthorizedAccessException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            catch (FileNotFoundException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            return result;
        }
        #endregion

        #region FillDestinationItemsList
        private void FillDestinationItemsList(string destinationPath, ref List<DestinationItem> list, Guid parentGuid = default(Guid))
        {
            var directories = Directory.GetDirectories(destinationPath, "*.*", SearchOption.TopDirectoryOnly);
            var files = Directory.GetFiles(destinationPath, "*.*", SearchOption.TopDirectoryOnly);

            var dInfo = new DirectoryInfo(destinationPath);

            var newDirectoryGuid = Guid.NewGuid();
            list.Add(new DestinationItem()
            {
                UniqGuid = newDirectoryGuid,
                ParentGuid = parentGuid,
                EntityPath = dInfo.Root.Name,
                EntityType = 1,
                Title = dInfo.FullName
            });

            foreach (var file in files)
            {
                var fInfo = new FileInfo(file);
                list.Add(new DestinationItem()
                {
                    UniqGuid = Guid.NewGuid(),
                    ParentGuid = parentGuid
                    ,
                    EntityExtension = fInfo.Extension
                    ,
                    EntityType = 2
                    ,
                    EntityPath = fInfo.DirectoryName
                    ,
                    Title = fInfo.FullName
                    ,
                    FileSize = fInfo.Length
                    ,
                    Checksum = Utilites.Security.ComputeMD5Checksum(fInfo.FullName)

                });
            }

            foreach (var directory in directories)
            {
                FillDestinationItemsList(directory, ref list, newDirectoryGuid);
            }

        }



        #endregion

        #region Execute
        public void Execute()
        {
            var result = CreateDestinationList();

            _dm.BulkCopyArchiveEntity(_cnf.GetConnectionString(), result.Result, _imgInfoParams.DriveId);

            var dest = new Destination(_imgInfoParams.DriveId, result.Result);

            if (_imgInfoParams.FillImages)
            {
                FillImageInfo(result.Result.Where(x => _fm.IsImage(x.EntityExtension)).ToList());
            }
            if (_imgInfoParams.FillMedia)
            {
                FillMediaInfo(result.Result.Where(x => _fm.IsMedia(x.EntityExtension)).ToList());
            }
        }
        #endregion
        public void FillImageInfo(List<DestinationItem> items)
        {
            //TODO: Добавление картинок, если выбрана , если надо сохранять в БД
            /* 
             Создать временный каталог сохранить картинки
             Сформировать Список картинок
             Добавить Картинки в БД
             Скопировать в основной каталог
             */

            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            var listImage = new List<ImageDto>();
            // Папка с описанием диска
            var baseDrivePath = string.Format(@"drive{0}", _imgInfoParams.DriveId);
            var drivePathTmp = Path.Combine(tempDirectory, baseDrivePath);
            foreach (var item in items)
            {
                var fi = new FileInfo(item.Title);
                string NewImgPath = "";


                    if (!Directory.Exists(drivePathTmp))
                    {
                        Directory.CreateDirectory(drivePathTmp);
                    }
                    NewImgPath = Path.Combine(drivePathTmp, item.UniqGuid.ToString()+ fi.Extension);
                    if (!File.Exists(NewImgPath))
                    {
                        File.Copy(item.Title, NewImgPath, true);
                    }


                var NewThumbDirPath = Path.Combine(drivePathTmp, _cnf.GetThumbDirName());
                var NewThumbPath = Path.Combine(NewThumbDirPath, item.UniqGuid.ToString()+ fi.Extension);
                
                // Создание эскиза
                Bitmap bmp = _fm.GetThumb(item.Title);

                if (!Directory.Exists(NewThumbDirPath))
                {
                    Directory.CreateDirectory(NewThumbDirPath);
                }
                var imageInfo = new FileInfo(item.Title);
                var imageData = _fm.GetImageData(bmp);
                listImage.Add( new ImageDto() { HashCode= imageInfo.GetHashCode(),
                UniqGuid =item.UniqGuid,
                ImagePath = Path.Combine(baseDrivePath, item.UniqGuid.ToString() + fi.Extension),
                ImageTitle = imageInfo.Name,
                Thumbnail = imageData,
                ThumbnailPath = Path.Combine(baseDrivePath, _cnf.GetThumbDirName(), item.UniqGuid.ToString() + fi.Extension)
                });
                bmp.Save(NewThumbPath);
            }

            _dm.BulkCopyImage(_cnf.GetConnectionString(), listImage);
            // После добавления в БД
            Copy(drivePathTmp, _cnf.GetTargetImagePath());
            Directory.Delete(drivePathTmp);


        }




        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles())
            {
               
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }






        private string CopyImg(string imgPath, string targetPath)
        {
            #region Guard
            if (string.IsNullOrWhiteSpace(imgPath))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(imgPath));
            if (string.IsNullOrWhiteSpace(targetPath))
                throw new ArgumentException(ERROR_ARGUMENT_EXCEPTION_MSG, nameof(targetPath));
            #endregion
            var result = "";
            try
            {
                var fi = new FileInfo(imgPath);
                string NewimgPath = "";

                if (_fm.IsImage(fi.Extension))
                {
                    // var dir = Path.Combine(_cnf.GetTargetImagePath(), targetPath);
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    NewimgPath = Path.Combine(targetPath, fi.Name);
                    if (!File.Exists(Path.Combine(targetPath, fi.Name)))
                    {
                        File.Copy(imgPath, NewimgPath, true);
                    }

                }
                result = string.IsNullOrWhiteSpace(NewimgPath) ? "" : Path.Combine(targetPath, fi.Name);
            }
            catch (Exception e)
            {
                _dm.logger.Add(string.Format("Ошибка в методе CopyImg. {0}", e.Message));
                throw new Exception("Ошибка в методе CopyImg");
            }


            return result;
        }

        public void FillMediaInfo(List<DestinationItem> items)
        {
            //TODO: Добавление Медиа информации, если выбрана
        }
    }
}
