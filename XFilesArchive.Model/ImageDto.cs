using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Model
{
    public class ImageDto
    {
        public byte[] Thumbnail { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string ImageTitle { get; set; }
        public Nullable<int> HashCode { get; set; }
        public Guid? UniqGuid { get; set; }
    }
}
