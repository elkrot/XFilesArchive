using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Repositories
{
    public class CategoryRepository:  GenericRepository<Category, XFilesArchiveDataContext>, ICategoryRepository
    {
        public CategoryRepository(XFilesArchiveDataContext context) : base(context)
        {
        }

        public void AddCategoryToEntities(Category category, List<int> entities)
        {
            if (category == null) throw new ArgumentException("category");
            var values = "";
            var categoryKey = 0;
            if (category.CategoryKey == 0)
            {
                this.Add(category);
                Save();

            }
            categoryKey = category.CategoryKey;

            if (categoryKey != 0)
            {
                values = string.Join(",", entities.Select(x => string.Format("({0},{1})", x, categoryKey)).ToArray());
                Context.Database.ExecuteSqlCommand(string.Format("insert into CategoryToEntity (TargetEntityKey,CategoryKey) values {0}", values));
            }
        }

        public List<Category> GetAllCategories()
        {
            return  Context.Set<Category>().ToList();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await Context.Set<Category>().ToListAsync();
        }

        public async Task<List<Drive>> GetAllDriveAsync()
        {
            return await Context.Set<Drive>()
                .ToListAsync();
        }

        public Category GetCategoryByKey(int? categoryKey)
        {
            return Context.Set<Category>().FirstOrDefault(x => x.CategoryKey == categoryKey);
        }

        public async Task ReloadDriveAsync(int? id)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Drive>()
                .SingleOrDefault(db => db.Entity.DriveId == id);
            if (dbEntityEntry != null)
            {
                await dbEntityEntry.ReloadAsync();
            }
        }
    }
}
