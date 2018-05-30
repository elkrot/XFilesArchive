using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using XFilesArchive.Model;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Lookups;

namespace XFilesArchive.UI.ViewModel.Navigation
{
    public interface IFilesOnDriveNavigationViewModel
    {
        void Load(int? DriveId = default(int?));
    }

    public class FilesOnDriveNavigationViewModel : IFilesOnDriveNavigationViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ITreeViewLookupProvider<ArchiveEntity> _fileOnDriveLookupProvider;

        public FilesOnDriveNavigationViewModel(IEventAggregator eventAggregator,
          ITreeViewLookupProvider<ArchiveEntity> fileOnDriveLookupProvider)
        {
            _eventAggregator = eventAggregator;
            _fileOnDriveLookupProvider = fileOnDriveLookupProvider;

            _eventAggregator.GetEvent<FileOnDriveSavedEvent>().Subscribe(OnFileOnDriveSaved);
            _eventAggregator.GetEvent<FileOnDriveDeletedEvent>().Subscribe(OnFileOnDriveDeleted);

            NavigationItems = new ObservableCollection<NavigationTreeItemViewModel>();

            SelectedItemChangedCommand = 
                new DelegateCommand<int>(OnSelectedItemChangedCommandExecute, OnSelectedItemChangedCommandCanExecute);

        }

        private bool OnSelectedItemChangedCommandCanExecute(int arg)
        {
            return true;
        }


        private void OnSelectedItemChangedCommandExecute(int obj)
        {
            if (obj != 0)
            {
                var SelectedItem = 0;
                int.TryParse(obj.ToString(), out SelectedItem);

                _eventAggregator.GetEvent<SelectedItemChangedEvent>().Publish(SelectedItem);
            }
        }

        public ICommand SelectedItemChangedCommand { get; private set; }


        private void OnFileOnDriveDeleted(int archiveEntityKey)
        {
            var navigationItem =
               NavigationItems.SingleOrDefault(item => item.ArchiveEntityKey == archiveEntityKey);
            if (navigationItem != null)
            {
                NavigationItems.Remove(navigationItem);
            }
        }

        private void OnFileOnDriveSaved(ArchiveEntity savedDrive)
        {
            var navigationItem =
               NavigationItems.SingleOrDefault(item => item.ArchiveEntityKey == savedDrive.ArchiveEntityKey);
            if (navigationItem != null)
            {
                navigationItem.DisplayValue = string.Format("{0}", savedDrive.Title.Trim()
                    );
            }
            else
            {
                Load();
            }
        }

        public void Load(int? DriveId = default(int?))
        {
            IEnumerable<LookupItemNode> items;

            items = _fileOnDriveLookupProvider.GetLookup(DriveId);

            NavigationItems.Clear();
            foreach (var driveLookupItem in
                items)
            {
                NavigationItems.Add(
                  new NavigationTreeItemViewModel(
                    driveLookupItem,
                    _eventAggregator));
            }
        }
        public ObservableCollection<NavigationTreeItemViewModel> NavigationItems { get; set; }





    }
    //--------------------------------------------------------------------
    
}
