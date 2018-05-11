using System.Collections.Generic;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Data
{
    public interface ILookupDataService
    {
        Task<IEnumerable<LookupItem>> GetDriveLookupAsync();
    }
}