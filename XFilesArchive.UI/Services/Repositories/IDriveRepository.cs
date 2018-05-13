using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Repositories
{


    public interface IDriveRepository: IGenericRepository<Drive>
    {

        void RemoveFile(ArchiveEntity model);
        Task<bool> HasFileAsync(int id);
       

    }
}