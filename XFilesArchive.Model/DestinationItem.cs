using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Model
{
    public class DestinationItem
    {
        public string Title { get; set; }
        public byte EntityType { get; set; }
        public string EntityPath { get; set; }
        public string EntityExtension { get; set; }
        public Nullable<long> FileSize { get; set; }
        public string Checksum { get; set; }
        
        public Guid UniqGuid { get; set; }
        public Guid ParentGuid { get; set; }
    }
}
