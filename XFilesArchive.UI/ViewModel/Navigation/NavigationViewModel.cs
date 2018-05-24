using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Lookups;
using System;
using System.Windows;

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

        public ObservableCollection<NavigationItemViewModel> Drives { get; }

        public ObservableCollection<NavigationItemViewModel> Categories { get; }

        public NavigationViewModel(ILookupDataService lookupDataService
            , ICategoryLookupDataService categoryLookupDataService
            , IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _lookupDataService = lookupDataService;

            _categoryLookupDataService = categoryLookupDataService;
            Categories = new ObservableCollection<NavigationItemViewModel>();

            Drives = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterSaveEvent>().Subscribe(AfterSaved);
            _eventAggregator.GetEvent<AfterDeletedEvent>().Subscribe(AfterDeleted);

            FirstPageCommand = new DelegateCommand(FirstPageCommandExecute);
            PrevPageCommand = new DelegateCommand(PrevPageCommandExecute);
            NextPageCommand = new DelegateCommand(NextPageCommandExecute);
            LastPageCommand = new DelegateCommand(LastPageCommandExecute);

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

        private void AfterDetailDelited(ObservableCollection<NavigationItemViewModel> items
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

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items
            , AfterDriveSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName
                    , _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }


        /*
                     get { return (int)GetValue(CurrentPageProperty); }
                    set { SetValue(CurrentPageProperty, value); }
                     */
        #region CurrentPage


        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(NavigationViewModel)
                , new PropertyMetadata(0, CurrentPage_Changed));

        private async static void CurrentPage_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as NavigationViewModel;
            if (current != null)
            {
               await  current.LoadAsync();
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
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set
            {
                SetValue(FilterTextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(NavigationViewModel),
                new PropertyMetadata(String.Empty, FilterText_Changed));

        private async static void FilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as NavigationViewModel;
            if (current != null)
            {
               await current.LoadAsync();
            }
        }
        #endregion





        public async Task LoadAsync()
        {
            //var lookup = await _lookupDataService.GetDriveLookupAsync();


            itemsCount = await _lookupDataService.GetDrivesCountByConditionAsync(x => x.Title.Contains(FilterText), x => x.DriveCode
                , false, 1, int.MaxValue);

            var lookup = await _lookupDataService.GetDrivesByConditionAsync(x => x.Title.Contains(FilterText), x => x.DriveCode
            , false, CurrentPage, PageLength);



            Drives.Clear();
            foreach (var item in lookup)
            {
                Drives.Add(new NavigationItemViewModel(item.DriveId, item.Title, nameof(DriveDetailViewModel), _eventAggregator));
            }


            var categoryLookup = await _categoryLookupDataService.GetCategoryLookupAsync();
            Categories.Clear();
            foreach (var item in categoryLookup)
            {
                //Categories.Add(new NavigationItemViewModel(item.Id,
                //item.DisplayMember, nameof(CategoryDetailViewModel), _eventAggregator));
            }


        }



    }

}
