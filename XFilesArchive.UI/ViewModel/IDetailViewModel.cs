using System.Threading.Tasks;

namespace XFilesArchive.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int id);
        bool HasChanges { get; }
        int Id { get;  }
    }
}
