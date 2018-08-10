using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Lookups
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




        public async Task<IEnumerable<Drive>> GetDrivesByConditionAsync(
           Expression<Func<Drive, bool>> where
           , Expression<Func<Drive, object>> orderby
           )
        {
            using (var context = _contextCreator())
            {
                {
                    return await context.Drives.Where(where).OrderBy(orderby).ToListAsync<Drive>();
                }
            }
        }

        public async Task<IEnumerable<Drive>> GetDrivesByConditionAsync(
            Expression<Func<Drive, bool>> where
            , Expression<Func<Drive, object>> orderby
            , bool isDescending, int index, int length)
        {
            using (var context = _contextCreator())
            {
                {
                    var skip = (index - 1) * length;
                    return await context.Drives.Where(where).OrderBy(orderby).Skip(skip).Take(length).ToListAsync<Drive>();
                }
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

        public async Task<int> GetDrivesCountByConditionAsync(Expression<Func<Drive, bool>> where, Expression<Func<Drive, object>> orderby, bool isDescending, int index, int length)
        {
            using (var context = _contextCreator())
            {
                {
                    return await context.Drives.Where(where).OrderBy(orderby).CountAsync();
                }
            }
        }

        public IEnumerable<Drive> GetDrivesByCondition(Expression<Func<Drive, bool>> where, Expression<Func<Drive, object>> orderby, bool isDescending, int index, int length)
        {
            using (var context = _contextCreator())
            {
                {
                    var skip = (index - 1) * length;
                    return  context.Drives.Where(where).OrderBy(orderby).Skip(skip).Take(length).ToList();
                }
            }
        }

        public int GetDrivesCountByCondition(Expression<Func<Drive, bool>> where, Expression<Func<Drive, object>> orderby, bool isDescending, int index, int length)
        {
            using (var context = _contextCreator())
            {
                {
                    return  context.Drives.Where(where).OrderBy(orderby).Count();
                }
            }
        }

        public IEnumerable<ArchiveEntity> GetAllFilesOnDrive(int id)
        {
            using (var context = _contextCreator())
            {
                {
                    return context.ArchiveEntities.Where(x=>x.DriveId==id);
                }
            }
        }

        public IEnumerable<LookupItem> GetTagLookup()
        {
            using (var context = _contextCreator())
            {
                return context.Tags.AsNoTracking()
                    .Select(f => new LookupItem { Id = f.TagKey, DisplayMember = f.TagTitle })
                    .ToList();
            }
        }
    }
}
