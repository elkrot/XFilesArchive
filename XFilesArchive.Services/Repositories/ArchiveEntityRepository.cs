using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Repositories
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

        public ICollection<ArchiveEntity> GetEntitiesByCondition(Expression<Func<ArchiveEntity, bool>> condition)
        {
            return Context.ArchiveEntities.Where(condition).ToList();
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

        public void RemoveImage(int archiveEntityKey, int imageKey)
        {
            var entity = Context.ArchiveEntities.Find(archiveEntityKey);
            var image = Context.Images.Where(x => x.ImageKey == imageKey).First();
            if (image != null)
            {
                entity.Images.Remove(image);
            }
        }

        public void RemoveTag(int archiveEntityKey, string tagTitle)
        {
            var entity = Context.ArchiveEntities.Find(archiveEntityKey);
            var tag = Context.Tags.Where(x=>x.TagTitle ==tagTitle).First();
            if (tag != null)
            {
                entity.Tags.Remove(tag);
            }
        }
    }
}
