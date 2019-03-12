using Autofac;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.Model;
using XFilesArchive.UI.Event;
using XFilesArchive.Services.Lookups;
using XFilesArchive.Services.Repositories;
using XFilesArchive.UI.Startup;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.ViewModel.Navigation;
using XFilesArchive.UI.Wrapper;
using System;
using XFilesArchive.Infrastructure;
using XFilesArchive.Infrastructure.DataManager;
using System.IO;

namespace XFilesArchive.UI.ViewModel
{
    public class DriveDetailViewModel : DetailViewModelBase, IDriveDetailViewModel
    {
        private IDriveRepository _repository;

        private DriveWrapper _drive;
        private IEnumerable<ArchiveEntity> _allFilesOnDrive;
        private FilesOnDriveViewModel _filesOnDriveViewModel;
        private readonly IEventAggregator _eventAggregator;
        public FilesOnDriveViewModel FilesOnDriveViewModel
        {
            get { return _filesOnDriveViewModel; }
        }

        public ICommand  SelectedItemChangedCommand { get; }
        public ICommand DeleteAllImagesCommand { get; }
        public DriveDetailViewModel(IDriveRepository repository, IEventAggregator eventAggregator
            , IMessageDialogService messageService,IAppLogger appLogger) : base(eventAggregator, messageService,appLogger)

        {
            _repository = repository;
            NavigationItems = new ObservableCollection<NavigationTreeItemViewModel>();
            //    ArchiveEntities = new ObservableCollection<ArchiveEntityWrapper>();

            _eventAggregator = eventAggregator;
            _allFilesOnDrive = new List<ArchiveEntity>();
            _eventAggregator.GetEvent<AfterCollectionSavedEvent>().Subscribe(AfterCollectionSaved);
            AddArchiveEntityCommand = new DelegateCommand(OnAddArchiveEntityExecute);
            RemoveArchiveEntityCommand = new DelegateCommand(OnRemoveArchiveEntityExecute,
            OnRemoveArchiveEntityCanExecute);
            CloseSearchDetailViewModelCommand = new DelegateCommand(OnCloseSearchDetailViewExecute);
            SelectedItemChangedCommand = new DelegateCommand<int?>(OnSelectedItemChangedExecute,OnSelectedItemChangedCanExecute);
            DeleteAllImagesCommand = new DelegateCommand(OnDeleteAllImagesExecute, OnDeleteAllImagesCanExecute);
        }

        private bool OnDeleteAllImagesCanExecute()
        {
            return true;
        }

        private void OnDeleteAllImagesExecute()
        {
            var result = MessageDialogService.ShowOKCancelDialog("Удаление информации об изображениях.", 
                "Удалять информацию по сем картинкам в даном расположении?");
            if (result == MessageDialogResult.OK) {
                var cnf = new ConfigurationData();
                var lg = new Logger();
                var fm = new FileManager(cnf, lg);
                IDataManager dm = new DataManager(cnf, fm, lg, 0);
                var dmResult = dm.RemoveImagesFromDrive(this.Id);
                if (dmResult.Success) {
                    var path = Path.Combine(cnf.GetTargetImagePath(),string.Format("drive{0}",Id));
                    fm.DeleteDirectory(path);
                }

                //TODO: Удаление из БД
                //TODO: Удаление в файловой системе
            }

        }

        public ICommand CloseSearchDetailViewModelCommand { get; private set; }
        private void OnCloseSearchDetailViewExecute()
        {
            _eventAggregator.GetEvent<AfterSearchDetailClosedEvent>()
                .Publish(new AfterSearchDtailClosedEventArgs
                {
                    Id = this.Id
                    ,
                    ViewModelName = this.GetType().Name
                });
        }
        private bool OnSelectedItemChangedCanExecute(int? arg)
        {
            return true;
        }

        private async void   OnSelectedItemChangedExecute(int? arg)
        {
            if (arg != null)
            {
                await FilesOnDriveViewModel.LoadAsync((int)arg);
            }
            EventAggregator.GetEvent<SelectedItemChangedEvent>().Publish(arg??0);
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            //if (args.ViewModelName == nameof(ProgrammingLanguageDetailViewModel))
            //{
            //    await LoadProgrammingLanguagesLookup();
            //}

            await Task.Factory.StartNew(() => { });
        }

        private bool OnRemoveArchiveEntityCanExecute()
        {
            return SelectedArchiveEntity != null;
        }

        private void OnRemoveArchiveEntityExecute()
        {
            SelectedArchiveEntity.PropertyChanged -= Wrapper_PropertyChanged;
            _repository.RemoveFile(SelectedArchiveEntity.Model);
            Drive.Model.ArchiveEntities.Remove(SelectedArchiveEntity.Model);
         //   ArchiveEntities.Remove(SelectedArchiveEntity);
            SelectedArchiveEntity = null;
            HasChanges = _repository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddArchiveEntityExecute()
        {
            var newArchiveEntity = new ArchiveEntityWrapper(new ArchiveEntity());
            newArchiveEntity.PropertyChanged += Wrapper_PropertyChanged;
            //ArchiveEntities.Add(newArchiveEntity);
            Drive.Model.ArchiveEntities.Add(newArchiveEntity.Model);
            newArchiveEntity.Title = "";

        }

        protected override async void OnDeleteExecute()
        {
            //if (await _repository.HasMeetingAsync(Drive.DriveId))
            //{
            //    await MessageDialogService.ShowInfoDialogAsync("!!!");
            //    return;
            //}
            var result = await MessageDialogService.ShowOKCancelDialogAsync("Удалить расположение?", "Удаление информации о расположении");
            if (result == MessageDialogResult.OK)
            {
                _repository.Remove(Drive.Model);
                await _repository.SaveAsync();
                RaiseDetailDelitedEvent(Drive.DriveId);
            }
        }

        private bool OnDeleteCanExecute()
        {
            return true;
        }

        public DriveWrapper Drive
        {
            get { return _drive; }
            private set
            {
                _drive = value;
                OnPropertyChanged();
            }
        }



        public ICommand AddArchiveEntityCommand { get; }
        public ICommand RemoveArchiveEntityCommand { get; }

        protected override bool OnSaveCanExecute()
        {
            return Drive != null && !Drive.HasErrors
                && HasChanges;


//&& ArchiveEntities.All(q => !q.HasErrors)

        }



        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_repository.SaveAsync, () =>
            {

                HasChanges = _repository.HasChanges();
                Id = Drive.DriveId;
                RaiseDetailSavedEvent(Drive.DriveId, $"{Drive.Title}");
            });
        }

        public ArchiveEntityWrapper SelectedArchiveEntity
        {
            get
            {
                return _selectedArchiveEntity;
            }
            set
            {
                _selectedArchiveEntity = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveArchiveEntityCommand).RaiseCanExecuteChanged();
            }
        }

        public ArchiveEntityWrapper _selectedArchiveEntity { get; set; }

        public override async Task LoadAsync(int id)
        {

            var drive = id > 0 ?
                await _repository.GetByIdAsync(id) :
                CreateNewDrive();
            _allFilesOnDrive = _repository.GetAllFilesOnDriveById(id);
            Id = id;
            InitializedDrive(drive);
            InitializeArchiveEntitys();

            var bootstrapper = new Bootstrapper();
            IContainer container = bootstrapper.Bootstrap();
            Title = Drive.Title;
            _filesOnDriveViewModel = container.Resolve<FilesOnDriveViewModel>();
            
           // _filesOnDriveViewModel.Load(Id);


            //     await LoadProgrammingLanguagesLookup();Drive.Model.ArchiveEntities
        }

        private void InitializeArchiveEntitys()
        {
            //ICollection<ArchiveEntity> archiveEntities
            //foreach (var wrapper in ArchiveEntities)
            //{
            //    wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            //}
            //ArchiveEntities.Clear();
            //foreach (var ArchiveEntity in archiveEntities)
            //{
            //    var wrapper = new ArchiveEntityWrapper(ArchiveEntity);
            //    ArchiveEntities.Add(wrapper);
            //    wrapper.PropertyChanged += Wrapper_PropertyChanged;
            //}

            var lookup = GetLookup();

            NavigationItems.Clear();
            foreach (var driveLookupItem in lookup)
            {
                NavigationItems.Add( new NavigationTreeItemViewModel(driveLookupItem,
                    EventAggregator));
            }





        }





        private ObservableCollection<LookupItemNode> GetNodesById(int archiveEntityKey)
        {

            return new ObservableCollection<LookupItemNode>(
                _allFilesOnDrive.Where(x => x.ParentEntityKey == archiveEntityKey)
                .OrderBy(l => l.EntityType)
                .Select(
                f => new LookupItemNode
                {
                    Id = f.ArchiveEntityKey,
                    DisplayMember = string.Format("{0}", f.Title),
                    Nodes = GetNodesById(f.ArchiveEntityKey),
                    EntityType = f.EntityType
                }
                ).ToList());

        }

        public IEnumerable<LookupItemNode> GetLookup()
        {


            var ret = _allFilesOnDrive.Where(x => x.ParentEntityKey == null)
            .OrderBy(l => l.EntityType)
                    .Select(f => new LookupItemNode
                    {
                        Id = f.ArchiveEntityKey,
                        DisplayMember = string.Format("{0}", f.Title),
                        Nodes = GetNodesById(f.ArchiveEntityKey),
                        EntityType = f.EntityType
                    })
                    .ToList();
            return ret;

        }




        private void Wrapper_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _repository.HasChanges();
            }
            if (e.PropertyName == nameof(ArchiveEntityWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializedDrive(Model.Drive drive)
        {
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

                if (e.PropertyName == nameof(Drive.Title))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Drive.DriveId == 0)
            {
                Drive.Title = "";
            }
        }

        private void SetTitle()
        {
            Title = $"{Drive.Title}";
        }

        private async Task LoadProgrammingLanguagesLookup()
        {
            //ProgrammingLanguages.Clear();
            //ProgrammingLanguages.Add(new NullLookupItem());
            //var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            //foreach (var lookupItem in lookup)
            //{
            //    ProgrammingLanguages.Add(lookupItem);
            //}
            await Task.Factory.StartNew(() => { });
        }

        private Model.Drive CreateNewDrive()
        {
            var drive = new Model.Drive();
            _repository.Add(drive);
            return drive;
        }


public ObservableCollection<NavigationTreeItemViewModel> NavigationItems { get; set; }



        /*
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
        
         
         */



    }
}
