using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Lookups
{
   public class LookupDataService : ILookupDataService
        , ICategoryLookupDataService
        ,ITagLookupDataService
    {
        private Func<XFilesArchiveDataContext> _contextCreator;

        public LookupDataService(Func<XFilesArchiveDataContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetDriveLookupAsync() {
            using (var context = _contextCreator()) {
                return await context.Drives.AsNoTracking()
                    .Select(f => new LookupItem { Id = f.DriveId, DisplayMember = f.Title })
                    .ToListAsync() ;
            }
        }


        public async Task<IEnumerable<LookupItem>> GetCategoryLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Categories.AsNoTracking()
                    .Select(f => new LookupItem { Id = f.CategoryKey, DisplayMember = f.CategoryTitle })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetTagLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Tags.AsNoTracking()
                    .Select(f => new LookupItem { Id = f.TagKey, DisplayMember = f.TagTitle })
                    .ToListAsync();
            }
        }


    }
}
