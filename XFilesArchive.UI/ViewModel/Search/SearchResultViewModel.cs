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
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;

        public string Title { get {return string.Format("Результат поиска::{0}", Id);} }

        public SearchResultViewModel(IEventAggregator eventAggregator
            , IMessageDialogService messageDialogService
            )
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SearchResult = new SearchResult(new List<ArchiveEntity>());
            CloseSearchDetailViewModelCommand = new DelegateCommand(OnCloseDetailViewExecute);
            OpenSearchResultArchiveEntityCommand= new DelegateCommand<int?>(OnOpenSearchResultArchiveEntityExecute);
            OpenSearchResultDriveCommand = new DelegateCommand<int?>(OnOpenSearchResultDriveExecute);
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

   

        }

        public void Load(SearchResult searchResult,int id)
        {
            SearchResult = searchResult;
            _id = id;
        }


    }
}
