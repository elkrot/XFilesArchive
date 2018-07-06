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
using System.Windows.Input;
using XFilesArchive.Infrastructure;
using System.Linq;
using Microsoft.Win32;
using XFilesArchive.UI.ViewModel.Services;

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
            try
            {
AddTagCommand = new DelegateCommand<int?>(OnAddTagExecute, OnAddTagCanExecute);
            }
            catch (Exception ex)
            {

                throw;
            }
 

        }


        #region Конструктор


        private async void OnSelectedItemChanged(int obj)
        {
            if (obj != 0)
            {
                int.TryParse(obj.ToString(), out int ArchiveEntityKey);
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


        public ICommand OpenFileDialogCommand { get; private set; }

        public ICommand AddTagCommand { get; private set; }

        public ICommand AddCategoryCommand { get; private set; }

        public ICommand AddNewCategoryCommand { get; private set; }

        public ICommand DeleteTagCommand { get; private set; }

        public ICommand DeleteImageCommand { get; private set; }

        public ICommand DeleteCategoryToEntityCommand { get; private set; }






        private bool OnDeleteCategoryToEntityCanExecute(object arg)
        {
            return true;
        }
        delegate MethodResult<int> RemoveEntityCollectionItem(int ArchiveEntityKey, int CollectionKey);

        private async void RemoveItemFromEntityCollection(RemoveEntityCollectionItem action, int CollectionKey)
        {
            var saveRet = action(ArchiveEntity.ArchiveEntityKey, CollectionKey);

            if (!saveRet.Success)
            {
                 await _messageDialogService.ShowInfoDialogAsync(
   
    string.Format("Во время сохранения записи {0}{2} возникла исключительная ситуация{2}  {1}"
    , ArchiveEntity.Title, saveRet.Messages.FirstOrDefault(), Environment.NewLine));
              //  ArchiveEntity.RejectChanges();
            }
            else
            {
              //  ArchiveEntity.AcceptChanges();
            }
            _eventAggregator.GetEvent<FileOnDriveSavedEvent>().Publish(ArchiveEntity.Model);
            InvalidateCommands();
        }

        private void OnDeleteCategoryToEntityExecute(object obj)
        {
            int CategoryKey = (int)obj;
            CategoryWrapper categoryW = ArchiveEntity.Categories.Where(x => x.CategoryKey == CategoryKey).First();
            ArchiveEntity.Categories.Remove(categoryW);
          //  RemoveItemFromEntityCollection(_fileOnDriveDataProvider.RemoveCategoryFromEntity, CategoryKey);
        }

        private bool OnDeleteImageCanExecute(object arg)
        {
            return true;
        }

        private void OnDeleteImageExecute(object obj)
        {
            int ImageKey = (int)obj;
            ImageWrapper image = ArchiveEntity.Images.Where(x => x.ImageKey == ImageKey).First();
            ArchiveEntity.Images.Remove(image);
            ///   Дописать удаление файла !!!!!!!!!!!!
         //   RemoveItemFromEntityCollection(_fileOnDriveDataProvider.RemoveImageFromEntity, ImageKey);

        }

        private bool OnAddNewCategoryCanExecute(object arg)
        {
            return true;
        }

        private void OnAddNewCategoryExecute(object obj)
        {
            var strkey = obj.ToString();

            AddCategoryViewModel vm = new AddCategoryViewModel()
            {
                CategoryTitle = "Новая категория"
            };

            int.TryParse(strkey, out int parentKey);


            AddCategoryDialog dlg = new AddCategoryDialog()
            {
                DataContext = vm
            };
            var result = dlg.ShowDialog();

            if (result == true)
            {
                Category category = new Category() { CategoryTitle = vm.CategoryTitle };
                if (parentKey > 0)
                {
                    category.ParentCategoryKey = parentKey;
                }

              //  _categoryDataProvider.AddCategory(category);
                CategoryNavigationViewModel.Load();
            }

        }

        #region OnOpenFileDialog
        /// <summary>
        /// Добавление картинки к сущности
        /// </summary>
        /// <param name="obj"></param>
        private void OnOpenFileDialogExecute(object obj)
        {
            OpenFileDialog myDialog = new OpenFileDialog()
            {
                Filter = "Картинки(*.JPG;*.GIF;*.PNG)|*.JPG;*.GIF;*.PNG" + "|Все файлы (*.*)|*.* ",
                CheckFileExists = true,
                Multiselect = true
            };
            if (myDialog.ShowDialog() == true)
            {
                //var ret = _fileOnDriveDataProvider.AddImageToFileOnDrive(ArchiveEntity.Model.ArchiveEntityKey
                //    , myDialog.FileName, (int)ArchiveEntity.Model.DriveId);

                //var imageKey = ret.Result;

                //if (ret.Success)
                //{

                //    var img = _fileOnDriveDataProvider.GetImageToEntityById(ArchiveEntity.Model.ArchiveEntityKey,
                //        ret.Result);
                //    var imgw = new ImageWrapper(img);
                //    ArchiveEntity.Images.Add(imgw);
                //    ArchiveEntity.Images.AcceptChanges();
                //}
            }
        }

        private bool OnOpenFileDialogCanExecute(object arg)
        {//errrororororor
            return true;
        }

        #endregion


        #region OnAddTag
        private void OnAddTagExecute(int? obj)
        {
            //var ret = _fileOnDriveDataProvider.AddTagToEntity(ArchiveEntity.Model.ArchiveEntityKey
            //    , obj.ToString());

            //if (ret.Success)
            //{
            //    var tag = _fileOnDriveDataProvider.GetTagToEntityById(ArchiveEntity.Model.ArchiveEntityKey,
            //        ret.Result);
            //    if (tag != null)
            //    {
            //        var tagw = new TagWrapper(tag);

            //        ArchiveEntity.Tags.Add(tagw);
            //        ArchiveEntity.Tags.AcceptChanges();
            //    }


            //}
        }


        private bool OnAddTagCanExecute(int? arg)
        {//errrororororor
            return true;
        }
        #endregion

        #region OnDeleteTag
        private void OnDeleteTagExecute(object obj)
        {
            var TagKey = (int)obj;
            TagWrapper tag = ArchiveEntity.Tags.Where(x => x.TagKey == TagKey).First();
            ArchiveEntity.Tags.Remove(tag);
          //  RemoveItemFromEntityCollection(_fileOnDriveDataProvider.RemoveTagFromEntity, TagKey);
        }
        private bool OnDeleteTagCanExecute(object arg)
        {//errrororororor
            return true;
        }
        #endregion

        #region OnAddCategory
        private void OnAddCategoryExecute(object obj)
        {
            //var CategoryId = 0;
            //Int32.TryParse(obj.ToString(), out CategoryId);
            //var category = _fileOnDriveDataProvider.GetCategoryToEntityById(ArchiveEntity.Model.ArchiveEntityKey,
            //        CategoryId);

            //if (category == null)
            //{
            //    var ret = _fileOnDriveDataProvider.AddCategoryToEntity(ArchiveEntity.Model.ArchiveEntityKey
            //        , CategoryId);

            //    if (ret.Success)
            //    {
            //        category = _fileOnDriveDataProvider.GetCategoryToEntityById(ArchiveEntity.Model.ArchiveEntityKey,
            //           ret.Result);
            //    }
            //}
            //var categoryew = new CategoryWrapper(category);
            //ArchiveEntity.Categories.Add(categoryew);
            //ArchiveEntity.Categories.AcceptChanges();
        }

        private bool OnAddCategoryCanExecute(object arg)
        {//errrororororor
            return true;
        }
        #endregion

    }

}