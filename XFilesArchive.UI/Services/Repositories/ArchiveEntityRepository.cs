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

        public Category GetCategoryById(int id)
        {
            return Context.Categories.Find(id);
        }

        public Tag GetTagByTitle(string Title)
        {
            var tag = Context.Tags.Where(x => x.TagTitle == Title).FirstOrDefault();
            if (tag==null)
            {
                tag = new Tag() { TagTitle =Title};
            }
            return tag;
        }

        public void RemoveCategory(int ArchiveEntityId,int categoryKey)
        {
            var entity = Context.ArchiveEntities.Find(ArchiveEntityId);
            var category = Context.Categories.Find(categoryKey);
            entity.Categories.Remove(category);
        }
    }
}
