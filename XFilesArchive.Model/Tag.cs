using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XFilesArchive.Model
{
    public partial class Tag
    {
        public Tag()
        {
            this.ArchiveEntities = new HashSet<ArchiveEntity>();
        }

        [Key]
        public int TagKey { get; set; }

        [StringLength(50)]
        public string TagTitle { get; set; }

        public System.DateTime ModififedDate { get; set; }
        public virtual ICollection<ArchiveEntity> ArchiveEntities { get; set; }
    }
}
