using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;
using XFilesArchive.UI.Services;

namespace XFilesArchive.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IArchiveDataService _archiveDataService;

        public INavigationViewModel NavigationViewModel { get; set; }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public MainViewModel(INavigationViewModel navigationViewModel)
        {
            NavigationViewModel = navigationViewModel;

        }

    }
}
