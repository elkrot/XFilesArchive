using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.UI.Services.Repositories
{
    public interface IArchiveEntityRepository :IGenericRepository<ArchiveEntity>
    {
        Category GetCategoryById(int id);
        Tag GetTagByTitle(string Title);
    }
}
