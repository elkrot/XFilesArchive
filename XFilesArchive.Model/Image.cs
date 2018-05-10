using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XFilesArchive.Model
{
    public partial class Image
    {
        public Image()
        {
            this.ArchiveEntities = new HashSet<ArchiveEntity>();
        }

        [Key]
        public int ImageKey { get; set; }

        public byte[] Thumbnail { get; set; }

        [StringLength(255)]
        public string ImagePath { get; set; }

        [StringLength(255)]
        public string ThumbnailPath { get; set; }

        [Required]
        [StringLength(100)]
        public string ImageTitle { get; set; }

        public Nullable<int> HashCode { get; set; }

        public System.DateTime CreatedDate { get; set; }
        
        public virtual ICollection<ArchiveEntity> ArchiveEntities { get; set; }
    }
}
