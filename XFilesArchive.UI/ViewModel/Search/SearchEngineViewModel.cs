using Autofac.Features.Indexed;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel.Search
{
    public class SearchEngineViewModel : Observable
    {

        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;

        private IDetailViewModel _selectedDetailViewModel;
        private IIndex<string, IDetailViewModel> _searchDetailViewModelCreator;

        public ISearchNavigationViewModel SearchNavigationViewModel { get; private set; }
        // public ISearchResultViewModel SearchResultViewModel { get; private set; }

        public IDetailViewModel SelectedSearchDetailViewModel
        {
            get { return _selectedDetailViewModel; }
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IDetailViewModel> SearchDetailViewModels { get; }

        public SearchEngineViewModel(IEventAggregator eventAggregator
            , IMessageDialogService messageDialogService
            , ISearchNavigationViewModel searchNavigationViewModel
            //, ISearchResultViewModel searchResultViewModel
            , IIndex<string, IDetailViewModel> searchDetailViewModelCreator
           )
        {
            _searchDetailViewModelCreator = searchDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            //_eventAggregator.GetEvent<ShowSearchResultEvent>().Subscribe(OnShowSearchResult);

            _eventAggregator.GetEvent<OpenSearchDetailViewEvent>()
    .Subscribe(OnOpenDetailView);

            _eventAggregator.GetEvent<OpenSearchDetailArchiveEntityViewEvent>()
    .Subscribe(OnOpenDetailArchiveEntityView);


            _eventAggregator.GetEvent<AfterDetailClosedEvent>()
.Subscribe(OnAfterDetailClosed);

            SearchNavigationViewModel = searchNavigationViewModel;
            //SearchResultViewModel = searchResultViewModel;
            SearchDetailViewModels = new ObservableCollection<IDetailViewModel>();

        }

        private async void OnOpenDetailArchiveEntityView(OpenSearchDetailArchiveEntityViewEventArgs args)
        {

            IDetailViewModel detailViewModel = SearchDetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id
                && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                //TODO: Разобраться что куда Изменить алгоритм поиска
                detailViewModel = _searchDetailViewModelCreator[args.ViewModelName];
                //;
                //SearchNavigationViewModel.SearchResult
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch
                {
                    await _messageDialogService.ShowInfoDialogAsync("Info");
                    await SearchNavigationViewModel.LoadAsync();
                    return;
                }

                SearchDetailViewModels.Add(detailViewModel);
            }
            SelectedSearchDetailViewModel = detailViewModel;
        }

        private void OnAfterDetailClosed(AfterDtailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int? id, string viewModelName)
        {
            var detailViewModel = SearchDetailViewModels
    .SingleOrDefault(vm => vm.Id == id
    && vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                SearchDetailViewModels.Remove(detailViewModel);
            }
        }
        private int currentId = 0;
        private async void OnOpenDetailView(OpenSearchDetailViewEventArgs args)
        {
            //var detailViewModel = SearchDetailViewModels
            //    .SingleOrDefault(vm => vm.Id == args.Id
            //    && vm.GetType().Name == args.ViewModelName);

            //if (detailViewModel == null)
            //{
            //TODO: Разобраться что куда Изменить алгоритм поиска
            var detailViewModel = _searchDetailViewModelCreator[args.ViewModelName];
            //;
            //SearchNavigationViewModel.SearchResult
            try
            {
                if (args.ViewModelName == "SearchResultViewModel")
                {
                    ((SearchResultViewModel)detailViewModel).Load(SearchNavigationViewModel.SearchResult, ++currentId);
                }
                else
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
            }
            catch
            {
                await _messageDialogService.ShowInfoDialogAsync("Info");
                await SearchNavigationViewModel.LoadAsync();
                return;
            }

            SearchDetailViewModels.Add(detailViewModel);
            //}
            SelectedSearchDetailViewModel = detailViewModel;
        }

        private void OnShowSearchResult(int obj)
        {
            //SearchResultViewModel.SearchResult = SearchNavigationViewModel.SearchResult;
            // SearchResultViewModel.Load();
        }

        public void Load()
        {
            SearchNavigationViewModel.Load();
            //  SearchResultViewModel.Load();
        }

        public async Task LoadAsync()
        {
            await SearchNavigationViewModel.LoadAsync();
        }

    }
}
