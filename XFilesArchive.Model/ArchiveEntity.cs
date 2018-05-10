using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XFilesArchive.Model
{
    public partial class ArchiveEntity
    {
        public ArchiveEntity()
        {
            this.ArchiveEntities = new List<ArchiveEntity>();
            this.Categories = new HashSet<Category>();
            this.Images = new HashSet<Image>();
            this.Tags = new HashSet<Tag>();
        }

        [Key]
        public int ArchiveEntityKey { get; set; }

        public Nullable<int> ParentEntityKey { get; set; }

        public Nullable<int> DriveId { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        public byte EntityType { get; set; }

        [StringLength(250)]
        public string EntityPath { get; set; }

        [StringLength(20)]
        public string EntityExtension { get; set; }

        public string Description { get; set; }

        public Nullable<long> FileSize { get; set; }

        public byte[] EntityInfo { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public byte[] MFileInfo { get; set; }

        public string Checksum { get; set; }

        //virtual
        public virtual ICollection<ArchiveEntity> ArchiveEntities { get; set; }
        public virtual ArchiveEntity ParentEntity { get; set; }
        public virtual Drive Drive { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
