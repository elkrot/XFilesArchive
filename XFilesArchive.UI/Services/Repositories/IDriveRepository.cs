using System.Collections.Generic;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Repositories
{


    public interface IDriveRepository: IGenericRepository<Drive>
    {

        void RemoveFile(ArchiveEntity model);
        Task<bool> HasFileAsync(int id);
        IEnumerable<ArchiveEntity> GetAllFilesOnDriveById(int id);
    }
}