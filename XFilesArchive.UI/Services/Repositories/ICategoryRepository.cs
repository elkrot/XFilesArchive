using System.Collections.Generic;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Drive>> GetAllDriveAsync();
        Task ReloadDriveAsync(int? id);
        List<Category> GetAllCategories();
    }
}
