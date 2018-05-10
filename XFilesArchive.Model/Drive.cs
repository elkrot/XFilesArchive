using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XFilesArchive.Model
{
    public partial class Drive
    {
        public Drive()
        {
            this.ArchiveEntities = new List<ArchiveEntity>();
        }

        [Key]
        public int DriveId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public Nullable<int> HashCode { get; set; }

        [Required]
        public byte[] DriveInfo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [Required]
        [StringLength(20)]
         public string DriveCode { get; set; }
        public virtual ICollection<ArchiveEntity> ArchiveEntities { get; set; }
        public Boolean IsSecret { get; set; }
    }
}
