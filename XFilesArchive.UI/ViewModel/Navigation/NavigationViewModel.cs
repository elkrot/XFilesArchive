using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.Services.Lookups;

namespace XFilesArchive.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
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

        public async Task LoadAsync()
        {
            var lookup = await _lookupDataService.GetDriveLookupAsync();
            Drives.Clear();
            foreach (var item in lookup)
            {
                Drives.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(DriveDetailViewModel), _eventAggregator));
            }


            lookup = await _categoryLookupDataService.GetCategoryLookupAsync();
            Categories.Clear();
            foreach (var item in lookup)
            {
                //Categories.Add(new NavigationItemViewModel(item.Id,
                //    item.DisplayMember, nameof(CategoryDetailViewModel), _eventAggregator));
            }


        }



    }

}
