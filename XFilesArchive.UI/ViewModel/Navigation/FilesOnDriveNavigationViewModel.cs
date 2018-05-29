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
    public class NavigationTreeItemViewModel : Observable
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayValue;

        #region Constructor
        public NavigationTreeItemViewModel(LookupItemNode itemNode,
          IEventAggregator eventAggregator)
        {
            ArchiveEntityKey = itemNode.Id;
            DisplayValue = itemNode.DisplayMember;
            NavigationItems = GetItems(itemNode.Nodes);
            ImagePath = itemNode.EntityType == 1 ? "/HomeArchiveX.WpfUI;component/img/folder.png"
                : "/HomeArchiveX.WpfUI;component/img/document_empty.png";
            //;
            _eventAggregator = eventAggregator;
            //OpenFileOnDriveEditViewCommand = new DelegateCommand(OpenFileOnDriveEditViewExecute);



        }

        private ObservableCollection<NavigationTreeItemViewModel> GetItems(ObservableCollection<LookupItemNode> nodes)
        {
            var retItems = new ObservableCollection<NavigationTreeItemViewModel>();
            foreach (var item in nodes)
            {
                var itm = new NavigationTreeItemViewModel(
                    item,
                    _eventAggregator);
                retItems.Add(itm);

            }
            return retItems;
        }
        #endregion


        // public ICommand OpenFileOnDriveEditViewCommand { get; set; }

        public int ArchiveEntityKey { get; private set; }
        public ObservableCollection<NavigationTreeItemViewModel> NavigationItems { get; set; }
        public string ImagePath { get; set; }
        #region DisplayValue
        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value.Trim();
                OnPropertyChanged();
            }
        }
        #endregion

        #region OpenDriveEditViewExecute
        //private void OpenFileOnDriveEditViewExecute(object obj)
        //{
        //    _eventAggregator.GetEvent<OpenFileOnDriveEditViewEvent>().Publish(ArchiveEntityKey);

        //}


        #endregion

    }
}
