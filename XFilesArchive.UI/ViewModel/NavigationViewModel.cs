using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;
using XFilesArchive.UI.Services.Data;

namespace XFilesArchive.UI.ViewModel
{
    public class NavigationViewModel : INavigationViewModel
    {
        private ILookupDataService _lookupDataService;

        public NavigationViewModel(ILookupDataService lookupDataService)
        {
            _lookupDataService = lookupDataService;
            Drives = new ObservableCollection<LookupItem>();
        }

        public ObservableCollection<LookupItem> Drives{ get; }

        public async Task LoadAsync()
        {
            var lookup = await _lookupDataService.GetDriveLookupAsync();
            Drives.Clear();
            foreach (var item in lookup)
            {
                Drives.Add(item);
            }
        }
    }

}
