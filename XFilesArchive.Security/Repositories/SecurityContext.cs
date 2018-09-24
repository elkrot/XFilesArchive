using System.Data.Entity;
using XFilesArchive.Security.Models.Mapping;

namespace XFilesArchive.Security
{
    public partial class SecurityContext : DbContext
    {
        public SecurityContext()
            : base("name=SecurityContext")
        {
        }

        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
