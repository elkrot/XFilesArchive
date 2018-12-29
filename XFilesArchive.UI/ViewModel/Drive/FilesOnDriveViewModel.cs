using System;
using Prism.Events;
using XFilesArchive.UI.Event;
using XFilesArchive.Services.Repositories;
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
using HomeArchiveX.Infrastructure;
using System.IO;
using System.Drawing;
using XFilesArchive.Services.Lookups;
using System.Text;


namespace XFilesArchive.UI.ViewModel
{


    public class FilesOnDriveViewModel : DetailViewModelBase, IFilesOnDriveViewModel
    {
        private readonly IArchiveEntityRepository _repository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private ArchiveEntityWrapper _archiveEntity;
        delegate MethodResult<int> RemoveEntityCollectionItem(int ArchiveEntityKey, int CollectionKey);
        ITagRepository _tagRepository;
        private int? DriveId;
        private readonly ICategoryRepository _categoryRepository;
        private ICategoryNavigationViewModel _categoryNavigationViewModel;

        public ICategoryNavigationViewModel CategoryNavigationViewModel { get { return _categoryNavigationViewModel; } }
        public ObservableCollection<TagWrapper> Tags { get; }
        public ObservableCollection<CategoryWrapper> Categories { get; }
        public ObservableCollection<ImageWrapper> Images { get; }
        public List<string> TagsItems { get { return _tagRepository.TagsLookup().ToList(); } }

        public ICommand CloseSearchDetailViewModelCommand { get; private set; }
        public ICommand OpenFileDialogCommand { get; private set; }
        public ICommand AddTagCommand { get; private set; }
        public ICommand MultyAddTagCommand { get; private set; }
        public ICommand AddCategoryCommand { get; private set; }
        public ICommand AddNewCategoryCommand { get; private set; }
        public ICommand DeleteTagCommand { get; private set; }
        public ICommand DeleteImageCommand { get; private set; }
        public ICommand DeleteCategoryToEntityCommand { get; private set; }
        public ICommand EditDescriptionCommand { get; private set; }

        #region Constructor
        public FilesOnDriveViewModel(
                   IEventAggregator eventAggregator
                   , IMessageDialogService messageDialogService
                   , IArchiveEntityRepository repository
                   , ICategoryNavigationViewModel categoryNavigationViewModel
                   , ICategoryRepository categoryRepository
                   , ITagRepository tagRepository
                   ) : base(eventAggregator, messageDialogService)
        {
            _categoryRepository = categoryRepository;
            _categoryNavigationViewModel = categoryNavigationViewModel;
            _tagRepository = tagRepository;
            _repository = repository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<SelectedItemChangedEvent>().Subscribe(OnSelectedItemChanged);

            Tags = new ObservableCollection<TagWrapper>();
            Categories = new ObservableCollection<CategoryWrapper>();
            Images = new ObservableCollection<ImageWrapper>();

            CategoryNavigationViewModel.Load();

            #region Commands
            AddTagCommand = new DelegateCommand<string>(OnAddTagExecute, OnAddTagCanExecute);

            MultyAddTagCommand = new DelegateCommand<string>(OnAddMultyTagExecute, OnAddMultyTagCanExecute);

            AddCategoryCommand = new DelegateCommand<int?>(OnAddCategoryExecute, OnAddCategoryCanExecute);
            AddNewCategoryCommand = new DelegateCommand<int?>(OnAddNewCategoryExecute, OnAddNewCategoryCanExecute);
            OpenFileDialogCommand = new DelegateCommand(OnOpenFileDialogExecute, OnOpenFileDialogCanExecute);
            DeleteTagCommand = new DelegateCommand<string>(OnDeleteTagExecute, OnDeleteTagCanExecute);
            DeleteImageCommand = new DelegateCommand<int?>(OnDeleteImageExecute, OnDeleteImageCanExecute);
            DeleteCategoryToEntityCommand = new DelegateCommand<int?>(OnDeleteCategoryToEntityExecute, OnDeleteCategoryToEntityCanExecute);
            CloseSearchDetailViewModelCommand = new DelegateCommand(OnCloseSearchDetailViewExecute);
            EditDescriptionCommand = new DelegateCommand(OnEditDescriptionViewExecute);
            #endregion
        }
        #endregion

        private bool OnAddMultyTagCanExecute(string arg)
        {
            return true;
        }

        private void OnAddMultyTagExecute(string obj)
        {
            MultySelectEntityesDialog dlg = new MultySelectEntityesDialog();
            var items = _repository.GetEntitiesByCondition(x => x.DriveId == DriveId && x.EntityType == 2);

            ObservableCollection<ArchiveEntityLookupDto> lookup = new ObservableCollection<ArchiveEntityLookupDto>();

            foreach (var item in items)
            {
                lookup.Add(new ArchiveEntityLookupDto() { ArchiveEntityKey = item.ArchiveEntityKey, EntityPath = item.EntityPath, prSel = false, Title = item.Title });
            }

            dlg.DataContext = new MultySeltEntityeecsViewModel(lookup);
            if (dlg.ShowDialog() == true)
            {
                var result = dlg.DataContext as MultySeltEntityeecsViewModel;
                StringBuilder sb = new StringBuilder();
                foreach (var item in result.Items.Where(x => x.prSel == true))
                {
                    sb.AppendLine(item.Title);
                }
                System.Windows.MessageBox.Show(sb.ToString());
            }

        }

        #region OnEditDescriptionViewExecute
        private void OnEditDescriptionViewExecute()
        {
            HasChanges = true;
            InvalidateCommands();
        }
        #endregion

        #region OnCloseSearchDetailViewExecute
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
        #endregion

        #region OnSelectedItemChanged
        private async void OnSelectedItemChanged(int obj)
        {
            if (obj != 0)
            {
                int.TryParse(obj.ToString(), out int ArchiveEntityKey);
                await LoadAsync(ArchiveEntityKey);
            }
        }
        #endregion

        #region InitializeTags
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
        #endregion

        #region InitializeCategories
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
        #endregion

        #region InitializeImages
        private void InitializeImages(ICollection<Model.Image> images)
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
        #endregion

        #region Wrapper_PropertyChanged
        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _repository.HasChanges();
            }
            if (e.PropertyName == nameof(TagWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                InvalidateCommands();
            }
        }
        #endregion

        #region InvalidateCommands
        private void InvalidateCommands()
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            // ((DelegateCommand)ResetCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
        }
        #endregion

        #region LoadAsync
        public override async Task LoadAsync(int id)
        {
            var _archEntity = id > 0 ?
                           await _repository.GetByIdAsync(id) :
                             new ArchiveEntity();

            _archiveEntity = new ArchiveEntityWrapper(_archEntity);
            _archiveEntity.PropertyChanged -= Wrapper_PropertyChanged;
            _archiveEntity.PropertyChanged += Wrapper_PropertyChanged;
            Title = _archEntity.Title;
            Id = _archEntity.ArchiveEntityKey;
            DriveId = _archEntity.DriveId;
            InitializeTags(_archEntity.Tags);
            InitializeCategories(_archEntity.Categories);
            InitializeImages(_archEntity.Images);
            OnPropertyChanged("ArchiveEntity");
        }

        #endregion

        #region OnDeleteExecute
        protected override void OnDeleteExecute()
        {
        }
        #endregion

        #region OnSaveCanExecute
        protected override bool OnSaveCanExecute()
        {
            return ArchiveEntity != null && !ArchiveEntity.HasErrors
                && HasChanges;
        }
        #endregion

        #region OnSaveExecute
        protected async override void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_repository.SaveAsync, () =>
            {
                HasChanges = _repository.HasChanges();
                Id = ArchiveEntity.ArchiveEntityKey;
                RaiseDetailSavedEvent(ArchiveEntity.ArchiveEntityKey, $"{ArchiveEntity.Title}");
            });

        }
        #endregion

        #region ArchiveEntity
        public ArchiveEntityWrapper ArchiveEntity
        {
            get
            {
                return _archiveEntity;
            }
            private set
            {
                _archiveEntity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region OnDeleteCategoryToEntityCanExecute
        private bool OnDeleteCategoryToEntityCanExecute(int? arg)
        {
            return true;
        }
        #endregion

        #region RemoveItemFromEntityCollection
        private async void RemoveItemFromEntityCollection(RemoveEntityCollectionItem action, int CollectionKey)
        {
            var saveRet = action(ArchiveEntity.ArchiveEntityKey, CollectionKey);

            if (!saveRet.Success)
            {
                await _messageDialogService.ShowInfoDialogAsync(

   string.Format("Во время сохранения записи {0}{2} возникла исключительная ситуация{2}  {1}"
   , ArchiveEntity.Title, saveRet.Messages.FirstOrDefault(), Environment.NewLine));

            }

            _eventAggregator.GetEvent<FileOnDriveSavedEvent>().Publish(ArchiveEntity.Model);
            InvalidateCommands();
        }

        #endregion

        #region OnDeleteCategoryToEntityExecute
        private void OnDeleteCategoryToEntityExecute(int? id)
        {
            int CategoryKey = (int)id;
            CategoryWrapper categoryW = ArchiveEntity.Categories.Where(x => x.CategoryKey == CategoryKey).First();

            var result = MessageDialogService.ShowOKCancelDialog($"Удалить Категорию {categoryW.CategoryTitle}?",
                $"Удалить Категорию {categoryW.CategoryTitle}?");

            if (result == MessageDialogResult.OK)
            {
                if (categoryW != null)
                {
                    ArchiveEntity.Categories.Remove(categoryW);
                    categoryW.PropertyChanged -= Wrapper_PropertyChanged;
                    var wrapper = Categories.Where(x => x.CategoryKey == CategoryKey).First();
                    Categories.Remove(wrapper);
                    _repository.RemoveCategory(ArchiveEntity.ArchiveEntityKey, CategoryKey);
                    HasChanges = ArchiveEntity != null && !ArchiveEntity.HasErrors;
                }
            }

            /*
             if (await _repository.HasMeetingAsync(Test.TestKey))
            {
                await MessageDialogService.ShowInfoDialogAsync("!!!");
                return;
            }

            var result = await MessageDialogService.ShowOKCancelDialogAsync("?", "title");

            if (result == MessageDialogResult.OK)
            {
                _repository.Remove(Test.Model);
                await _repository.SaveAsync();
                RaiseDetailDelitedEvent(Test.TestKey);

            }
             
             */


            //  RemoveItemFromEntityCollection(_fileOnDriveDataProvider.RemoveCategoryFromEntity, CategoryKey);
        }

        #endregion

        #region OnDeleteImageCanExecute
        private bool OnDeleteImageCanExecute(int? arg)
        {
            return true;
        }
        #endregion

        #region OnDeleteImageExecute

        private void OnDeleteImageExecute(int? id)
        {
            int ImageKey = (int)id;
            ImageWrapper image = ArchiveEntity.Images.Where(x => x.ImageKey == ImageKey).First();


            var result = MessageDialogService.ShowOKCancelDialog($"Удалить Изображение {image.ImageTitle}?",
                $"Удалить Изображение {image.ImageTitle}?");

            if (result == MessageDialogResult.OK)
            {
                if (image != null)
                {
                    ArchiveEntity.Images.Remove(image);


                    image.PropertyChanged -= Wrapper_PropertyChanged;
                    var wrapper = Images.Where(x => x.ImageKey == ImageKey).First();
                    Images.Remove(wrapper);
                    _repository.RemoveImage(ArchiveEntity.ArchiveEntityKey, ImageKey);
                    HasChanges = ArchiveEntity != null && !ArchiveEntity.HasErrors;

                }
            }
            ///   Дописать удаление файла !!!!!!!!!!!!
         //   RemoveItemFromEntityCollection(_fileOnDriveDataProvider.RemoveImageFromEntity, ImageKey);

        }

        #endregion

        #region OnAddNewCategoryCanExecute
        private bool OnAddNewCategoryCanExecute(int? arg)
        {
            return true;
        }

        #endregion

        #region OnAddNewCategoryExecute
        private void OnAddNewCategoryExecute(int? obj)
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

                _categoryRepository.Add(category);
                _categoryRepository.Save();
                CategoryNavigationViewModel.Load();
            }

        }

        #endregion

        #region OnOpenFileDialog
        /// <summary>
        /// Добавление картинки к сущности
        /// </summary>
        /// <param name="obj"></param>
        private void OnOpenFileDialogExecute()
        {
            OpenFileDialog myDialog = new OpenFileDialog()
            {
                Filter = "Картинки(*.JPG;*.GIF;*.PNG)|*.JPG;*.GIF;*.PNG" + "|Все файлы (*.*)|*.* ",
                CheckFileExists = true,
                Multiselect = true
            };
            if (myDialog.ShowDialog() == true)
            {
                var ret = AddImageToFileOnDrive(ArchiveEntity.Model.ArchiveEntityKey
                    , myDialog.FileName, (int)ArchiveEntity.Model.DriveId);

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

        private bool OnOpenFileDialogCanExecute()
        {//errrororororor
            return true;
        }

        #endregion

        #region OnAddTag
        private void OnAddTagExecute(string tagTitle)
        {

            if (!string.IsNullOrWhiteSpace(tagTitle))
            {
                var tag = _repository.GetTagByTitle(tagTitle);

                var wrapper = new TagWrapper(tag);
                Tags.Add(wrapper);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ArchiveEntity.Model.Tags.Add(wrapper.Model);
                HasChanges = ArchiveEntity != null && !ArchiveEntity.HasErrors;
            }

        }


        private bool OnAddTagCanExecute(string arg)
        {//errrororororor
            return true;
        }
        #endregion

        #region OnDeleteTag
        private void OnDeleteTagExecute(string tagTitle)
        {

            TagWrapper tag = ArchiveEntity.Tags.Where(x => x.TagTitle == tagTitle).First();

            var result = MessageDialogService.ShowOKCancelDialog($"Удалить метку {tagTitle}?", $"Удалить метку {tagTitle}?");

            if (result == MessageDialogResult.OK)
            {
                if (tag != null)
                {
                    ArchiveEntity.Tags.Remove(tag);
                    tag.PropertyChanged -= Wrapper_PropertyChanged;

                    var wrapper = Tags.Where(x => x.TagTitle == tagTitle).First();
                    Tags.Remove(wrapper);
                    _repository.RemoveTag(ArchiveEntity.ArchiveEntityKey, tagTitle);
                    HasChanges = ArchiveEntity != null && !ArchiveEntity.HasErrors;


                }

            }

            //  RemoveItemFromEntityCollection(_fileOnDriveDataProvider.RemoveTagFromEntity, TagKey);
        }
        private bool OnDeleteTagCanExecute(string arg)
        {//errrororororor
            return true;
        }
        #endregion

        #region OnAddCategory
        private void OnAddCategoryExecute(int? obj)
        {
            Int32.TryParse(obj.ToString(), out int CategoryId);
            if (CategoryId != 0)
            {
                var category = _repository.GetCategoryById(CategoryId);

                var wrapper = new CategoryWrapper(category);
                Categories.Add(wrapper);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ArchiveEntity.Model.Categories.Add(wrapper.Model);
                HasChanges = ArchiveEntity != null && !ArchiveEntity.HasErrors;
            }

            InvalidateCommands();
        }

        private bool OnAddCategoryCanExecute(int? arg)
        {//errrororororor
            return true;
        }
        #endregion

        #region Рисунки
        public MethodResult<int> AddImageToFileOnDrive(int ArchiveEntityKey, string img, int DriveId)
        {
            MethodResult<int> ret = new MethodResult<int>(0) { Success = true };
            var cnf = new ConfigurationData();
            var lg = new Logger();
            var fm = new FileManager(cnf, lg);
            int ImageKey = 0;
            // Сохранить изображение, Сохранить эскиз
            string targetDir = string.Format(@"drive{0}\img{1}", DriveId, ArchiveEntityKey);
            var im = CreateImage(img, targetDir, cnf, lg, fm);

            // Сохранить запись об изображении в БД

            var wrapper = new ImageWrapper(im);
            Images.Add(wrapper);
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            ArchiveEntity.Model.Images.Add(wrapper.Model);
            HasChanges = ArchiveEntity != null && !ArchiveEntity.HasErrors;
            ImageKey = im.ImageKey;
            return new MethodResult<int>(ImageKey);

        }
        #region Создать запись об изображении
        /// <summary>
        /// Создать запись об изображении
        /// </summary>
        /// <param name="imagePath">Путь к изопражению</param>
        /// <param name="targetDir">Путь назначение</param>
        /// <returns>Ключ рисунка</returns>
        private Model.Image CreateImage(string imagePath, string targetDir, IConfiguration cnf, ILogger lg, IFIleManager fm)
        {
            var imgInfo = new FileInfo(imagePath);
            var imgDirPath = imgInfo.Directory.FullName;
            string newImgPath = "";
            byte[] imageData = null;
            try
            {
                newImgPath = fm.CopyImg(imagePath, targetDir);
            }
            catch (IOException copyError)
            {
                lg.Add(copyError.Message);
            }

            Bitmap bmp = fm.GetThumb(imagePath);
            string thumbPath = fm.SaveThumb(targetDir, cnf.GetThumbDirName(), bmp, imgInfo.Name);
            imageData = fm.GetImageData(bmp);


            var im = new Model.Image()
            {
                HashCode = imgInfo.GetHashCode()
                ,
                ImagePath = newImgPath
                ,
                ImageTitle = imgInfo.Name
                ,
                ThumbnailPath = thumbPath
                ,
                Thumbnail = imageData
            };
            return im;

        }
        #endregion
        #endregion

    }

}

















