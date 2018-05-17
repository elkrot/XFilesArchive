using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using XFilesArchive.DataAccess.Models.Mapping;
using XFilesArchive.Model;

namespace XFilesArchive.DataAccess
{
    public class XFilesArchiveDataContext:DbContext
    {
        public XFilesArchiveDataContext():base("XFilesArchiveDataContext") {        }

        static XFilesArchiveDataContext()
        {
            Database.SetInitializer<XFilesArchiveDataContext>(null);
        }
//new CreateDatabaseIfNotExists<XFilesArchiveDataContext>()
        public DbSet<ArchiveEntity> ArchiveEntities { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Drive> Drives { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ArchiveEntityMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new DriveMap());
            modelBuilder.Configurations.Add(new ImageMap());
            modelBuilder.Configurations.Add(new TagMap());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
