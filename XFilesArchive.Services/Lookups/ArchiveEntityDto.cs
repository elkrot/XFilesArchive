using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Lookups
{
    public class ArchiveEntityDto
    {
        public int ArchiveEntityKey { get; set; }

        public Nullable<int> DriveId { get; set; }
        
        public string Title { get; set; }

        public byte EntityType { get; set; }
        
        public string EntityPath { get; set; }
        
        public string EntityExtension { get; set; }

        public string Description { get; set; }

        public Nullable<long> FileSize { get; set; }

        public virtual Drive Drive { get; set; }


    }
}
