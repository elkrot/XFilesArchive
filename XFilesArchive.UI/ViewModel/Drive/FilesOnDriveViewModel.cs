using System;
using Prism.Events;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Repositories;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.Wrapper;
using XFilesArchive.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Commands;
using XFilesArchive.UI.ViewModel.Drive;

namespace XFilesArchive.UI.ViewModel
{

    
      public class FilesOnDriveViewModel : DetailViewModelBase, IFilesOnDriveViewModel
    {
        private readonly IArchiveEntityRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private ArchiveEntityWrapper _archiveEntity;

        private readonly ICategoryRepository _categoryRepository;
        private ICategoryNavigationViewModel _categoryNavigationViewModel;


        public ICategoryNavigationViewModel CategoryNavigationViewModel { get { return _categoryNavigationViewModel; }  }


        public FilesOnDriveViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
                   IArchiveEntityRepository repository
                   , ICategoryNavigationViewModel categoryNavigationViewModel
            , ICategoryRepository categoryRepository
            ) : base(eventAggregator, messageDialogService)
        {
            _categoryRepository = categoryRepository;
            _categoryNavigationViewModel = categoryNavigationViewModel;

            _repository = repository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<SelectedItemChangedEvent>().Subscribe(OnSelectedItemChanged);
            Tags = new ObservableCollection<TagWrapper>();
            Categories = new ObservableCollection<CategoryWrapper>();
            Images = new ObservableCollection<ImageWrapper>();
            CategoryNavigationViewModel.Load();


        }


        #region Конструктор


        private async void OnSelectedItemChanged(int obj)
        {
            if (obj != 0)
            {
                int ArchiveEntityKey = 0;
                int.TryParse(obj.ToString(), out ArchiveEntityKey);
               await LoadAsync(ArchiveEntityKey);
            }
        }
        #endregion




        public ObservableCollection<TagWrapper> Tags { get; }
        public ObservableCollection<CategoryWrapper> Categories { get; }
        public ObservableCollection<ImageWrapper> Images { get; }

        private void InitializeTags(ICollection<Tag> tags)
        {
            foreach (var wrapper in Tags)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }
            Tags.Clear();
            foreach (var tag in tags)
            {
                var wrapper = new TagWrapper(tag);
                Tags.Add(wrapper);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
            }
        }


        private void InitializeCategories(ICollection<Category> categories)
        {
            foreach (var wrapper in Categories)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }
            Categories.Clear();
            foreach (var category in categories)
            {
                var wrapper = new CategoryWrapper(category);
                Categories.Add(wrapper);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
            }
        }

        private void InitializeImages(ICollection<Image> images)
        {
            foreach (var wrapper in Images)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }
            Images.Clear();
            foreach (var image in images)
            {
                var wrapper = new ImageWrapper(image);
                Images.Add(wrapper);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
            }
        }



        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _repository.HasChanges();
            }
            if (e.PropertyName == nameof(TagWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InvalidateCommands()
        {
            throw new NotImplementedException();
        }

        public override async Task LoadAsync(int id)
        {
            var _archEntity = id>0 ?
                           await _repository.GetByIdAsync(id) :
                             new ArchiveEntity();

            _archiveEntity = new ArchiveEntityWrapper(_archEntity);
            InitializeTags(_archEntity.Tags);
            InitializeCategories(_archEntity.Categories);
            InitializeImages(_archEntity.Images);
            OnPropertyChanged("ArchiveEntity");
        }

        protected override void OnDeleteExecute()
        {
                   }

        protected override bool OnSaveCanExecute()
        {
            return true;
        }

        protected override void OnSaveExecute()
        {
            
        }

        public ArchiveEntityWrapper ArchiveEntity
        {
            get
            {
                return _archiveEntity;
            }
        }




    }
     
}