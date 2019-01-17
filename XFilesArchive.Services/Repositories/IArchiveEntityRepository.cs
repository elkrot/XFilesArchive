using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;
using XFilesArchive.Services.Lookups;

namespace XFilesArchive.Services.Repositories
{
    public interface IArchiveEntityRepository :IGenericRepository<ArchiveEntity>
    {
        Category GetCategoryById(int id);
        Tag GetTagByTitle(string Title);
        void RemoveCategory(int ArchiveEntityId, int categoryKey);
        void RemoveTag(int archiveEntityKey, string tagTitle);
        void RemoveImage(int archiveEntityKey, int imageKey);
        ICollection<ArchiveEntityDto> GetEntitiesByCondition(Expression<Func<ArchiveEntity, bool>> condition);
        Tag GetTagByKey(int tagKey);
        ICollection<ArchiveEntityDto> GetEntitiesByCondition(Expression<Func<ArchiveEntity, bool>> condition,
            Expression<Func<ArchiveEntity, object>> orderby, int currentPage, int pageLength);
    }
}
