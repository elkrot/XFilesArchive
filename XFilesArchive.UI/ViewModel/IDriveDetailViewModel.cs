using System.Threading.Tasks;

namespace XFilesArchive.UI.ViewModel
{
    public interface IDriveDetailViewModel
    {
        Task LoadAsync(int Id);
    }
}