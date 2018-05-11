using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Data
{
    public class LookupDataService : ILookupDataService
    {
        private Func<XFilesArchiveDataContext> _contextCreator;

        public LookupDataService(Func<XFilesArchiveDataContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetDriveLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Drives.AsNoTracking()
                    .Select(f => new LookupItem { Id = f.DriveId, DisplayMember = f.Title })
                    .ToListAsync();
            }
        }
    }

}
