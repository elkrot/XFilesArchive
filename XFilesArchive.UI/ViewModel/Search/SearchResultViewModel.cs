using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XFilesArchive.Model;
using XFilesArchive.Search.Result;
using XFilesArchive.Services.Lookups;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel.Search
{






    public interface ISearchResultViewModel: IDetailViewModel
    {
        void Load();
        SearchResult SearchResult { get; set; }
    }
    public class SearchResultViewModel : DependencyObject, ISearchResultViewModel
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
        #region CurrentPage
        private int _currentPage;

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
            /*    OnPropertyChanged();
                Task newTask = new Task(async delegate () {
                    await LoadAsync();
                });
                newTask.RunSynchronously();*/


            }
        }



        #endregion


        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;

        public string Title { get {return string.Format("Результат поиска::{0}", Id);} }

        public SearchResultViewModel(IEventAggregator eventAggregator
            , IMessageDialogService messageDialogService
            )
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SearchResult = new SearchResult(new List<ArchiveEntityDto>());
            CloseSearchDetailViewModelCommand = new DelegateCommand(OnCloseDetailViewExecute);
            OpenSearchResultArchiveEntityCommand= new DelegateCommand<int?>(OnOpenSearchResultArchiveEntityExecute);
            OpenSearchResultDriveCommand = new DelegateCommand<int?>(OnOpenSearchResultDriveExecute);

            FirstPageCommand = new Prism.Commands.DelegateCommand(FirstPageCommandExecute);
            PrevPageCommand = new Prism.Commands.DelegateCommand(PrevPageCommandExecute);
            NextPageCommand = new Prism.Commands.DelegateCommand(NextPageCommandExecute);
            LastPageCommand = new Prism.Commands.DelegateCommand(LastPageCommandExecute);


            _currentPage = 1;
            // SearchResult.MyProperty = 0;
        }

        private async void OnOpenSearchResultDriveExecute(int? id)
        {
            await Task.Factory.StartNew(() => {
                _eventAggregator.GetEvent<OpenSearchDetailDriveViewEvent>()
                .Publish(new OpenSearchDetailDriveViewEventArgs()
                { Id = (int)id, ViewModelName = nameof(DriveDetailViewModel) });

            });
        }

        private async void OnOpenSearchResultArchiveEntityExecute(int? id)
        {
            await Task.Factory.StartNew(() => {
                _eventAggregator.GetEvent<OpenSearchDetailArchiveEntityViewEvent>()
                .Publish(new OpenSearchDetailArchiveEntityViewEventArgs()
                { Id = (int)id, ViewModelName = nameof(FilesOnDriveViewModel) });
              
            });
        }

        private void OnCloseDetailViewExecute()
        {
            _eventAggregator.GetEvent<AfterSearchDetailClosedEvent>()
                .Publish(new AfterSearchDtailClosedEventArgs
                {
                    Id = this.Id
                    ,
                    ViewModelName = this.GetType().Name
                });
        }

        public ICommand CloseSearchDetailViewModelCommand { get; private set; }
        public ICommand OpenSearchResultArchiveEntityCommand { get; private set; }
        public ICommand OpenSearchResultDriveCommand { get; private set; }
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


        public SearchResult SearchResult
        {
            get { return (SearchResult)GetValue(SearchResultProperty); }
            set { SetValue(SearchResultProperty, value); }
        }

        public bool HasChanges { get { return false; } }

        private int _id;

        public int Id
        {
            get
            {
                return _id;
            }

            protected set
            {
                _id = value;

            }
        }


        public static readonly DependencyProperty SearchResultProperty =
           DependencyProperty.Register("SearchResult", typeof(SearchResult), typeof(SearchResultViewModel), new PropertyMetadata(null));

        public void Load()
        {

        }

        public async Task LoadAsync(int id)
        {

            await Task.Factory.StartNew(() => { });

        }

        public void Load(SearchResult searchResult,int id)
        {
            SearchResult = searchResult;
            _id = id;
        }


    }
}
