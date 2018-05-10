using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XFilesArchive.Model
{
    public partial class Category
    {
        public Category()
        {
            this.ArchiveEntities = new HashSet<ArchiveEntity>();
        }
        [Key]
        public int CategoryKey { get; set; }

        [StringLength(100)]
        public string CategoryTitle { get; set; }

        public Nullable<int> ParentCategoryKey { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public virtual ICollection<ArchiveEntity> ArchiveEntities { get; set; }
    }
}
