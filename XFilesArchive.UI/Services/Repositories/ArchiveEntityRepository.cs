using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Repositories
{
    public class ArchiveEntityRepository : GenericRepository<ArchiveEntity, XFilesArchiveDataContext>
        , IArchiveEntityRepository
    {
        public ArchiveEntityRepository(XFilesArchiveDataContext context) : base(context)
        {
        }
    }
}
