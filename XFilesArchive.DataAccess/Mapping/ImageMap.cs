using XFilesArchive.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace XFilesArchive.DataAccess.Models.Mapping
{
    public class ImageMap : EntityTypeConfiguration<Image>
    {
        public ImageMap()
        {
            this.ToTable("Image");
            this.Property(t => t.ImageKey).HasColumnName("ImageKey");
            this.Property(t => t.Thumbnail).HasColumnName("Thumbnail");
            this.Property(t => t.ImagePath).HasColumnName("ImagePath");
            this.Property(t => t.ThumbnailPath).HasColumnName("ThumbnailPath");
            this.Property(t => t.ImageTitle).HasColumnName("ImageTitle");
            this.Property(t => t.HashCode).HasColumnName("HashCode");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            this.HasMany<ArchiveEntity>(s => s.ArchiveEntities  )
                .WithMany(c => c.Images  )
                .Map(cs =>
                {
                    cs.MapLeftKey("ImageKey");
                    cs.MapRightKey("TargetEntityKey");
                    cs.ToTable("ImageToEntity");
                });
        }

    }
}
