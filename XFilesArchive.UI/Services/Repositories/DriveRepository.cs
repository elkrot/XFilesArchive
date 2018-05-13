using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Repositories
{
   public class DriveRepository : GenericRepository<Drive, XFilesArchiveDataContext>,IDriveRepository
    {
        public DriveRepository(XFilesArchiveDataContext context) : base(context)
        {
        }

        public override async Task<Drive> GetByIdAsync(int id)
        {
            return await Context.Drives.Include(t => t.ArchiveEntities).SingleAsync(t => t.DriveId == id);
        }

        public async Task<bool> HasFileAsync(int id) {
            return await Context.ArchiveEntities.AsNoTracking()
                .Include(mbox => mbox.ArchiveEntities)
                .AnyAsync(m => m.ArchiveEntities.Any(t => t.DriveId == id));
        }

        public void RemoveFile(ArchiveEntity model)
        {
            Context.ArchiveEntities.Remove(model);
        }




    }
}
 // await Task.Delay(5000);