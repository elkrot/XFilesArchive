using System.Collections.Generic;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Drive>> GetAllDriveAsync();
        Task ReloadDriveAsync(int? id);
        List<Category> GetAllCategories();
        Task<List<Category>> GetAllCategoriesAsync();
        Category GetCategoryByKey(int? categoryKey);
        void AddCategoryToEntities(Category category, List<int> entities);
    }
}
