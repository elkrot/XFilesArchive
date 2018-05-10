using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services
{
    public class ArchiveDataService : IArchiveDataService
    {
        private Func<XFilesArchiveDataContext> _contextCreator;

        public ArchiveDataService(Func<XFilesArchiveDataContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<Drive>> GetAllDrivesAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Drives.AsNoTracking().ToListAsync();
            }
        }

        public IEnumerable<Drive> GetAllDrives()
        {
            using (var context = _contextCreator())
            {
                return context.Drives.AsNoTracking().ToList();
            }

        }
    }
}
