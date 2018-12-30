using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.DataAccess;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Repositories
{
    public class TagRepository : GenericRepository<Tag, XFilesArchiveDataContext>, ITagRepository
    {
        public TagRepository(XFilesArchiveDataContext context) : base(context)
        {
        }

        public void AddTagToEntities(Tag tag, List<int> entities)
        { 
            if (tag == null) throw new ArgumentException("tag");
            var values = "";
            var tagKey = 0;
            if (tag.TagKey == 0)
            {
                this.Add(tag);
                Save();
                
            }
            tagKey = tag.TagKey;

                if (tagKey != 0) {
                values = string.Join(",",entities.Select(x => string.Format("({0},{1})", x, tagKey)).ToArray());
                Context.Database.ExecuteSqlCommand(string.Format("insert into TagToEntity (TargetEntityKey,TagKey) values {0}", values));
            }
        }

        public Tag GetTagByTitle(string tagTitle)
        {
            return Context.Tags.Where(x => x.TagTitle.Trim()==tagTitle).FirstOrDefault()??new Tag() { TagTitle=tagTitle};
        }

        public IEnumerable<string> TagsLookup()
        {
            return Context.Tags.Select(x => x.TagTitle.Trim()).ToArray();
        }
    }
}
