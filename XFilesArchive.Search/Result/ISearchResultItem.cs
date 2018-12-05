using XFilesArchive.Services.Lookups;

namespace XFilesArchive.Search.Result
{
    public interface ISearchResultItem
    {
        ArchiveEntityDto ArchiveEntity { get; }
    }
}
