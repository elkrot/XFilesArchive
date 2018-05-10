
using System.Data.Entity.ModelConfiguration;
using XFilesArchive.Model;

namespace XFilesArchive.DataAccess.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {

            // Table & Column Mappings
            this.ToTable("Category");
            this.Property(t => t.CategoryKey).HasColumnName("CategoryKey");
            this.Property(t => t.CategoryTitle).HasColumnName("CategoryTitle");
            this.Property(t => t.ParentCategoryKey).HasColumnName("ParentCategoryKey");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
