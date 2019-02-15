using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Model
{
    public class Destination
    {
        private IEnumerable<DestinationItem> _items;
        private int _driveId;

        public Destination(int driveId, IEnumerable<DestinationItem> items)
        {
            _driveId = driveId;
            _items = items;
        }
        public int DriveId { get { return _driveId; } }

        public IEnumerable<DestinationItem> Items { get { return _items; } }

        public void FillImageInfo()
        {
            //TODO: Добавление картинок, если выбрана , если надо сохранять в БД


            /*  dm.CreateImage
             *  
             *   newImgPath = _fileManager.CopyImg(imagePath, targetDir);
             *  
             *  Bitmap bmp = _fileManager.GetThumb(imagePath);
            string thumbPath = _fileManager.SaveThumb(targetDir, _configuration.GetThumbDirName(), bmp, imgInfo.Name);
            imageData = _fileManager.GetImageData(bmp);
             */
        }

        public void FillMediaInfo()
        {
            //TODO: Добавление Медиа информации, если выбрана
        }
    }

    public class FillInfoParameters {
        private string _destinationPath;
        private bool _fillImages;
        private bool _fillMedia;
        private int _driveId;

        public FillInfoParameters(string destinationPath, bool fillImages, bool fillMedia, int driveId)
        {
          _destinationPath= destinationPath;
          _fillImages = fillImages;
          _fillMedia= fillMedia;
          _driveId= driveId;
    }
        public string DestinationPath { get { return _destinationPath; } }
        public bool FillImages { get { return _fillImages; } }
        public bool FillMedia { get { return _fillMedia; } }
        public int DriveId { get { return _driveId; } }

    }


}
