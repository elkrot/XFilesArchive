using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Result
{
    public class SearchResultItem : ISearchResultItem
    {
        ArchiveEntity _archiveEntity;
        public ArchiveEntity ArchiveEntity { get { return _archiveEntity; } }

        public SearchResultItem(ArchiveEntity archiveEntity)
        {
            _archiveEntity = archiveEntity;
        }


    }
}
