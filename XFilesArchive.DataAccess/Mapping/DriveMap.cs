using System.Data.Entity.ModelConfiguration;
using XFilesArchive.Model;

namespace XFilesArchive.DataAccess.Models.Mapping
{
    public class DriveMap : EntityTypeConfiguration<Drive>
    {
        public DriveMap()
        {
            // Table & Column Mappings
            this.ToTable("Drive");
            this.Property(t => t.DriveId).HasColumnName("DriveId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.HashCode).HasColumnName("HashCode");
            this.Property(t => t.DriveInfo).HasColumnName("DriveInfo");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.DriveCode).HasColumnName("DriveCode");
        }
    }
}
