using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;

        public INavigationViewModel NavigationViewModel { get; }
        private Func<IDriveDetailViewModel> _driveDetailViewModelCreator { get; }
        private IDriveDetailViewModel _driveDetailViewModel;

        public IDriveDetailViewModel DriveDetailViewModel
        {
            get { return _driveDetailViewModel; }
            private set
            {
                _driveDetailViewModel = value;
                OnPropertyChanged();
            }
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private IMessageDialogService _messageDialogService;
        public MainViewModel(
                    INavigationViewModel navigationViewModel
                    , Func<IDriveDetailViewModel> driveDetailViewModelCreator
                    , IEventAggregator eventAggregator
                    , IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;


            _driveDetailViewModelCreator = driveDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenDriveDetailViewEvent>().Subscribe(OnOpenDriveDetailView);
            CreateNewDriveCommand = new DelegateCommand(OnCreateNewDriveExecute);
            NavigationViewModel = navigationViewModel;
        }
        private async void OnOpenDriveDetailView(int? id)
        {
            if (DriveDetailViewModel != null && DriveDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOKCancelDialog("?", "Q");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }

            }
            DriveDetailViewModel = _driveDetailViewModelCreator();
            await DriveDetailViewModel.LoadAsync(id);
        }


        public ICommand CreateNewDriveCommand { get; }
        
private void OnCreateNewDriveExecute()
        {
            OnOpenDriveDetailView(null);
        }


    }

}
