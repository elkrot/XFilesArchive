using System.Threading.Tasks;
using XFilesArchive.Search.Result;
using XFilesArchive.UI.ViewModel.Search;

namespace XFilesArchive.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int id);
        bool HasChanges { get; }
        int Id { get;  }
    }
}
