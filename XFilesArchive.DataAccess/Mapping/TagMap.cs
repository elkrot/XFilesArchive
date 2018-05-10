using XFilesArchive.Model;
using System.Data.Entity.ModelConfiguration;

namespace XFilesArchive.DataAccess.Models.Mapping
{
    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            // Table & Column Mappings
            this.ToTable("Tag");
            this.Property(t => t.TagKey).HasColumnName("TagKey");
            this.Property(t => t.TagTitle).HasColumnName("TagTitle");
            this.Property(t => t.ModififedDate).HasColumnName("ModififedDate");
        }
    }
}
