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
        public int DriveId { get { return _driveId; }  }

        public IEnumerable <DestinationItem> Items { get { return _items; }  }
    }


    

}
