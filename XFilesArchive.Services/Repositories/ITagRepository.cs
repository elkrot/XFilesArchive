using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        IEnumerable<string> TagsLookup();
    }
}
