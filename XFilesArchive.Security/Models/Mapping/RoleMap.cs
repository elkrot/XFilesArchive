using System.Data.Entity.ModelConfiguration;

namespace XFilesArchive.Security.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleId);

            this.Property(t => t.RoleTitle)
                .HasMaxLength(30);

            this.HasMany(e => e.User)
                .WithMany(e => e.Role)
                .Map(m => m.ToTable("UserToRole").MapLeftKey("RoleId").MapRightKey("UserId"));

            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.RoleTitle).HasColumnName("RoleTitle");

        }
    }
}
