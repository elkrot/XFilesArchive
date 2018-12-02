using System.Threading.Tasks;

namespace XFilesArchive.UI.ViewModel
{
    public interface INavigationViewModel
    {
        Task LoadAsync();
        void Load();
    }
}