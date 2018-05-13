using System.Collections.Generic;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Lookups
{
    public interface ITagLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetTagLookupAsync();
    }
}