using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.Model;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Repositories;
using XFilesArchive.UI.Wrapper;

namespace XFilesArchive.UI.ViewModel
{
    public class DriveDetailViewModel : ViewModelBase, IDriveDetailViewModel
    {
        private IDriveRepository _repository;
        private IEventAggregator _eventAggregator;

        public DriveDetailViewModel(IDriveRepository repository, IEventAggregator eventAggregator)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
             DeleteCommand = new DelegateCommand(OnDeleteExecute        
                        , OnDeleteCanExecute);
        }

        
        public ICommand SaveCommand { get; set; }
        private bool OnSaveCanExecute()
        {
            return Drive != null && !Drive.HasErrors && HasChanges;
        }

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public async Task LoadAsync(int? Id)
        {

            var drive = Id.HasValue ?
                await _repository.GetByIdAsync(Id.Value) :
                CreateNewDrive();

           
            Drive = new DriveWrapper(drive);
            Drive.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _repository.HasChanges();
                }

                if (e.PropertyName == nameof(Drive.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Drive.DriveId == 0)
            {
                Drive.Title = "";
            }

        }

        private Drive CreateNewDrive()
        {
            var drive = new Drive();
            _repository.Add(drive);
            return drive;
        }


        private async void OnSaveExecute()
        {
            await _repository.SaveAsync();
            HasChanges = _repository.HasChanges();

            _eventAggregator.GetEvent<AfterDriveSaveEvent>().Publish(new AfterDriveSavedEventArgs
            {
                Id = Drive.DriveId,
                DisplayMember = $"{Drive.Title}"
            });
        }

        private DriveWrapper _drive;

        public DriveWrapper Drive
        {
            get { return _drive; }
            private set
            {
                _drive = value;
                OnPropertyChanged();
            }
        }


        public ICommand DeleteCommand { get; }
       
        private async void OnDeleteExecute()
        {
            _repository.Remove(Drive.Model);
            await _repository.SaveAsync();

        }


    }
}
