using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Model
{
    public class Destination
    {
        public Destination(int driveId, IEnumerable<DestinationItem> items
            )
        {
            DriveId = driveId;
            Items = items;
        }
        public int DriveId { get; }

        public IEnumerable<DestinationItem> Items { get; }


    }

    public class FillInfoParameters
    {
        public FillInfoParameters(string destinationPath, bool fillImages, bool fillMedia, bool saveThumbnails,
            bool saveThumbnailsToDb, int driveId)
        {
            DestinationPath = destinationPath;
            FillImages = fillImages;
            FillMedia = fillMedia;
            DriveId = driveId;
            SaveThumbnailsToDb = saveThumbnailsToDb;
            SaveThumbnails = saveThumbnails;
        }
        public string DestinationPath { get; }
        public bool FillImages { get; }
        public bool FillMedia { get; }
        public bool SaveThumbnails { get; }
        public bool SaveThumbnailsToDb { get; }
        public int DriveId { get; }

    }


}
