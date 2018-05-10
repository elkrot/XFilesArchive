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

        public ObservableCollection<Drive> Drives { get; set; }
        private Drive _selectedDrive;

        public Drive SelectedDrive
        {
            get { return _selectedDrive; }
            set
            {
                _selectedDrive = value;
                OnPropertyChanged();
            }
        }
        public void Load()
        {
            var drives = _archiveDataService.GetAllDrives();
            foreach (var drive in drives)
            {
                Drives.Add(drive);
            }
        }
        public async Task LoadAsync()
        {
            var drives =await _archiveDataService.GetAllDrivesAsync();

            foreach (var drive in drives)
            {
                Drives.Add(drive);
            }
        }

        public MainViewModel(IArchiveDataService archiveDataService)
        {
            _archiveDataService = archiveDataService;
            Drives = new ObservableCollection<Drive>();
            SelectedDrive = new Drive();
        }
    }
}
