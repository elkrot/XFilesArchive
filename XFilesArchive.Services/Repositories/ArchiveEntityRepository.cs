using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;
using XFilesArchive.Services.Lookups;

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
        
        public ICollection<ArchiveEntityDto> GetEntitiesByCondition(Expression<Func<ArchiveEntity, bool>> condition)
        {
            var ret = Context.ArchiveEntities.AsExpandable()
                .AsNoTracking().Where(condition).Select
                (x=>new ArchiveEntityDto() {
                    ArchiveEntityKey = x.ArchiveEntityKey
                    ,Description = x.Description
                    ,Drive = x.Drive
                    ,DriveId  =x.DriveId
                    ,EntityExtension = x.EntityExtension
                    ,EntityPath = x.EntityPath
                    ,EntityType = x.EntityType
                    ,FileSize = x.FileSize
                    ,Title = x.Title
                }).ToList();
            return ret;
        }

        public ICollection<ArchiveEntityDto> GetEntitiesByCondition(Expression<Func<ArchiveEntity, bool>> condition, int currentPage, int pageLength)
        {
var skip = (currentPage - 1) * pageLength;

            var ret = Context.ArchiveEntities.AsExpandable()
               .AsNoTracking().Where(condition).Skip(skip).Select
               (x => new ArchiveEntityDto()
               {
                   ArchiveEntityKey = x.ArchiveEntityKey
                   ,
                   Description = x.Description
                   ,
                   Drive = x.Drive
                   ,
                   DriveId = x.DriveId
                   ,
                   EntityExtension = x.EntityExtension
                   ,
                   EntityPath = x.EntityPath
                   ,
                   EntityType = x.EntityType
                   ,
                   FileSize = x.FileSize
                   ,
                   Title = x.Title
               }).Take(pageLength).AsNoTracking().ToList();

            return ret;
        }

        public Tag GetTagByKey(int tagKey)
        {
            return Context.Tags.Where(x => x.TagKey == tagKey).FirstOrDefault();
            //.AsNoTracking()
        }

        public Tag GetTagByTitle(string Title)
        {
            var tag = Context.Tags.Where(x => x.TagTitle == Title).FirstOrDefault();
            //.AsNoTracking()
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
            var image = Context.Images.AsNoTracking().Where(x => x.ImageKey == imageKey).First();
            if (image != null)
            {
                entity.Images.Remove(image);
            }
        }

        public void RemoveTag(int archiveEntityKey, string tagTitle)
        {
            var entity = Context.ArchiveEntities.Find(archiveEntityKey);
            var tag = Context.Tags.AsNoTracking().Where(x=>x.TagTitle ==tagTitle).First();
            if (tag != null)
            {
                entity.Tags.Remove(tag);
            }
        }
    }
}
