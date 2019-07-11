using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.UI.Event;
using XFilesArchive.Services.Lookups;
using XFilesArchive.Security;
using System.Threading;
using System.Collections.Generic;
using System;

namespace XFilesArchive.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        public int itemsCount { get; set; }
        const int PAGE_LENGTH = 15;
        public int PageLength { get { return PAGE_LENGTH; } }
        public int TotalPages
        {
            get
            {
                var result = itemsCount / PageLength + (itemsCount % PageLength > 0 ? 1 : 0);
                return result;
            }
        }


        private IEventAggregator _eventAggregator;
        private ILookupDataService _lookupDataService;
        private ICategoryLookupDataService _categoryLookupDataService;

        public ObservableCollection<NavigationDriveItemViewModel> Drives { get; }

        public ObservableCollection<NavigationItemViewModel> Categories { get; }

        public NavigationViewModel(ILookupDataService lookupDataService
            , ICategoryLookupDataService categoryLookupDataService
            , IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _lookupDataService = lookupDataService;

            _categoryLookupDataService = categoryLookupDataService;
            Categories = new ObservableCollection<NavigationItemViewModel>();

            Drives = new ObservableCollection<NavigationDriveItemViewModel>();
            _eventAggregator.GetEvent<AfterSaveEvent>().Subscribe(AfterSaved);
            _eventAggregator.GetEvent<AfterDeletedEvent>().Subscribe(AfterDeleted);
            _eventAggregator.GetEvent<ShowHiddenEvent>().Subscribe(OnShowHidden);
            


            FirstPageCommand = new Prism.Commands.DelegateCommand(FirstPageCommandExecute);
            PrevPageCommand = new Prism.Commands.DelegateCommand(PrevPageCommandExecute);
            NextPageCommand = new Prism.Commands.DelegateCommand(NextPageCommandExecute);
            LastPageCommand = new Prism.Commands.DelegateCommand(LastPageCommandExecute);

            _currentPage = 1;
            _filterText = "";
        }

        private void OnShowHidden()
        {
            CanShowHidden=!CanShowHidden;
            Load();
        }

        private void AfterDeleted(AfterDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(DriveDetailViewModel):
                    AfterDetailDelited(Drives, args);
                    break;
                //case nameof(CategoryDetailViewModel):
                //    AfterDetailDelited(Categories, args);
                //    break;
            }
        }

        private void AfterDetailDelited(ObservableCollection<NavigationDriveItemViewModel> items
            , AfterDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(t => t.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterSaved(AfterDriveSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(DriveDetailViewModel):
                    AfterDetailSaved(Drives, args);
                    break;
                //case nameof(CategoryDetailViewModel):
                //    AfterDetailSaved(Categories, args);
                //    break;
            }

        }

        private void AfterDetailSaved(ObservableCollection<NavigationDriveItemViewModel> items
            , AfterDriveSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);


            if (lookupItem == null)
            {
                if (args.DriveCode != null)
                {
                    items.Add(new NavigationDriveItemViewModel(args.Id, args.DisplayMember,
                        args.ViewModelName
                        , _eventAggregator, args.DriveCode?.TrimEnd(' ') ?? "", args.IsSecret));
                }
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
                lookupItem.DriveCode = args.DriveCode;
            }
        }


        /*
                     get { return (int)GetValue(CurrentPageProperty); }
                    set { SetValue(CurrentPageProperty, value); }
                     */
        #region CurrentPage
        private int _currentPage;

        public int CurrentPage
        {
            get { return _currentPage;
            }
            set {
                _currentPage = value;
                OnPropertyChanged();
                Task newTask = new Task(async delegate () {
                    await LoadAsync();
                });
                newTask.RunSynchronously();


            }
        }



        #endregion

        public ICommand FirstPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }

        private void LastPageCommandExecute()
        {
            if (TotalPages > 1) CurrentPage = TotalPages;
        }

        private void NextPageCommandExecute()
        {
            if (TotalPages > CurrentPage) CurrentPage++;
        }

        private void PrevPageCommandExecute()
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        private void FirstPageCommandExecute()
        {
            CurrentPage = 1;
        }



        #region FilterText
        private string _filterText;
        public string FilterText
        {
            get {
                return _filterText;
            }
            set
            {
                _filterText = value;
                OnPropertyChanged();
                Task newTask = new Task(async delegate () {
                    await LoadAsync();
                });
                newTask.RunSynchronously();
            }
        }


        #endregion
        private bool _canShowHidden;
       public bool CanShowHidden { get {
                //        CustomPrincipal wp = Thread.CurrentPrincipal as CustomPrincipal;
                //        if (wp == null)
                //            return false;
                //        else
                //        return wp.IsInRole(@"Administrator");

                
                return _canShowHidden;
            }
            set {
                _canShowHidden = value;
            }
        }



        public async Task LoadAsync()
        {


               IEnumerable<DriveDto> lookup;
                if (CanShowHidden)
                {
               itemsCount = _lookupDataService.GetDrivesCountByCondition(x => x.Title.Contains(FilterText), x => x.DriveCode
               , false, 1, int.MaxValue);
               lookup = await _lookupDataService.GetDrivesByConditionAsync(x => x.Title.Contains(FilterText), 
               x => x.DriveCode, false, CurrentPage, PageLength);
                }
                else
                {
                    itemsCount = _lookupDataService
                        .GetDrivesCountByCondition(x => x.Title.Contains(FilterText)&& x.IsSecret==false, x => x.DriveCode
                        , false, 1, int.MaxValue);

                     lookup = await _lookupDataService.GetDrivesByConditionAsync(x => x.Title.Contains(FilterText) && x.IsSecret==false, x => x.DriveCode
                    , false, CurrentPage, PageLength);
                }
            

            Drives.Clear();
            foreach (var item in lookup)
            {
                Drives.Add(new NavigationDriveItemViewModel(item.DriveId,
                     string.Format("{0}", item.Title.TrimEnd(' ')
                        ), nameof(DriveDetailViewModel), _eventAggregator, item.DriveCode.TrimEnd(' '), item.IsSecret));
            }
        

            //var categoryLookup = await _categoryLookupDataService.GetCategoryLookupAsync();
            //Categories.Clear();
            //foreach (var item in categoryLookup)
            //{
            //    //Categories.Add(new NavigationItemViewModel(item.Id,
            //    //item.DisplayMember, nameof(CategoryDetailViewModel), _eventAggregator));
            //}


        }

        public void Load()
        {

                IEnumerable<DriveDto> lookup;

                if (CanShowHidden)
                {
                    itemsCount = _lookupDataService
               .GetDrivesCountByCondition(x => x.Title.Contains(FilterText), x => x.DriveCode
               , false, 1, int.MaxValue);

                    lookup = _lookupDataService.GetDrivesByCondition(x => x.Title.Contains(FilterText), x => x.DriveCode
                   , false, CurrentPage, PageLength);
                }
                else
                {
                    itemsCount = _lookupDataService
            .GetDrivesCountByCondition(x => x.Title.Contains(FilterText) && x.IsSecret==false, x => x.DriveCode
            , false, 1, int.MaxValue);

                    lookup = _lookupDataService.GetDrivesByCondition(x => x.Title.Contains(FilterText) && x.IsSecret==false, x => x.DriveCode
                   , false, CurrentPage, PageLength);
                }


                Drives.Clear();
                foreach (var item in lookup)
                {
                    Drives.Add(new NavigationDriveItemViewModel(
                        item.DriveId, string.Format("{0}",  item.Title.TrimEnd(' ')
                        ), nameof(DriveDetailViewModel), _eventAggregator, item.DriveCode.TrimEnd(' '), item.IsSecret));
                }

            
            //var categoryLookup =  _categoryLookupDataService.GetCategoryLookup();
            //Categories.Clear();
            //foreach (var item in categoryLookup)
            //{
            //    //Categories.Add(new NavigationItemViewModel(item.Id,
            //    //item.DisplayMember, nameof(CategoryDetailViewModel), _eventAggregator));
            //}
        }
    }

}
