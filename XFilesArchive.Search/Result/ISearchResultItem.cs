using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Result
{
    public interface ISearchResultItem
    {
        ArchiveEntity ArchiveEntity { get; }
    }
}
