
using System.Data.Entity.ModelConfiguration;
using XFilesArchive.Model;

namespace XFilesArchive.DataAccess.Models.Mapping
{
    public class ArchiveEntityMap : EntityTypeConfiguration<ArchiveEntity>
    {
        public ArchiveEntityMap()
        {
            // Table & Column Mappings
            this.ToTable("ArchiveEntity");
            this.Property(t => t.ArchiveEntityKey).HasColumnName("ArchiveEntityKey");
            this.Property(t => t.ParentEntityKey).HasColumnName("ParentEntityKey");
            this.Property(t => t.DriveId).HasColumnName("DriveId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.EntityPath).HasColumnName("EntityPath");
            this.Property(t => t.EntityExtension).HasColumnName("EntityExtension");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.FileSize).HasColumnName("FileSize");
            this.Property(t => t.EntityInfo).HasColumnName("EntityInfo");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.MFileInfo).HasColumnName("MFileInfo");
            this.Property(t => t.Checksum).HasColumnName("Checksum");

            // Relationships
            this.HasOptional(t => t.ParentEntity)
                .WithMany(t => t.ArchiveEntities)
                .HasForeignKey(d => d.ParentEntityKey);
            this.HasOptional(t => t.Drive)
                .WithMany(t => t.ArchiveEntities)
                .HasForeignKey(d => d.DriveId);

            this.HasMany<Tag>(s => s.Tags)
                .WithMany(c => c.ArchiveEntities)
                .Map(cs =>
                {
                    cs.MapLeftKey("TargetEntityKey");
                    cs.MapRightKey("TagKey");
                    cs.ToTable("TagToEntity");
                });


            this.HasMany<Image>(s => s.Images)
                .WithMany(c => c.ArchiveEntities)
                .Map(cs =>
                {
                    cs.MapLeftKey("TargetEntityKey");
                    cs.MapRightKey("ImageKey");
                    cs.ToTable("ImageToEntity");
                });


            this.HasMany<Category>(s => s.Categories)
                .WithMany(c => c.ArchiveEntities)
                .Map(cs =>
                {
                    cs.MapLeftKey("TargetEntityKey");
                    cs.MapRightKey("CategoryKey");
                    cs.ToTable("CategoryToEntity");
                });
        }
    }
}
