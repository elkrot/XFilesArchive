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

    public class FillInfoParameters {
        public FillInfoParameters(string destinationPath, bool fillImages, bool fillMedia, int driveId)
        {
          DestinationPath= destinationPath;
          FillImages = fillImages;
          FillMedia= fillMedia;
          DriveId= driveId;
    }
        public string DestinationPath { get; }
        public bool FillImages { get; }
        public bool FillMedia { get; }
        public int DriveId { get; }

    }


}
