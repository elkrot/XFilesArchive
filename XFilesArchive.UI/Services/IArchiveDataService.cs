using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services
{
    public interface IArchiveDataService
    {
        IEnumerable<Drive> GetAllDrives();
        Task<IEnumerable<Drive>> GetAllDrivesAsync();
    }
}
