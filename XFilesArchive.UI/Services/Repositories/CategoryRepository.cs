using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Repositories
{
    public class CategoryRepository:  GenericRepository<Category, XFilesArchiveDataContext>, ICategoryRepository
    {
        public CategoryRepository(XFilesArchiveDataContext context) : base(context)
        {
        }


        public List<Category> GetAllCategories()
        {
            return  Context.Set<Category>().ToList();
        }


        public async Task<List<Drive>> GetAllDriveAsync()
        {
            return await Context.Set<Drive>()
                .ToListAsync();
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
