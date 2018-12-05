using XFilesArchive.Model;
using XFilesArchive.Services.Lookups;

namespace XFilesArchive.Search.Result
{
    public class SearchResultItem : ISearchResultItem
    {
        ArchiveEntityDto _archiveEntity;
        public ArchiveEntityDto ArchiveEntity { get { return _archiveEntity; } }

        public SearchResultItem(ArchiveEntityDto archiveEntity)
        {
            _archiveEntity = archiveEntity;
        }


    }
}
