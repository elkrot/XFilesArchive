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

        public IEnumerable<string> TagsLookup()
        {
            return Context.Tags.Select(x => x.TagTitle.Trim()).ToArray();
        }
    }
}
