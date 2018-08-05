using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XFilesArchive.Model;
using XFilesArchive.UI.View;
using XFilesArchive.UI.View.Services;

namespace XFilesArchive.UI.ViewModel.Search
{
    public interface ISearchResultViewModel
    {
        void Load();
        SearchResult SearchResult { get; set; }
    }
    public class SearchResultViewModel : DependencyObject, ISearchResultViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;



        public SearchResultViewModel(IEventAggregator eventAggregator
            , IMessageDialogService messageDialogService
            )
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            SearchResult = new SearchResult(new List<ArchiveEntity>());

            // SearchResult.MyProperty = 0;
        }


        public SearchResult SearchResult
        {
            get { return (SearchResult)GetValue(SearchResultProperty); }
            set { SetValue(SearchResultProperty, value); }
        }

        public static readonly DependencyProperty SearchResultProperty =
           DependencyProperty.Register("SearchResult", typeof(SearchResult), typeof(SearchResultViewModel), new PropertyMetadata(null));

        public void Load()
        {

        }
    }
}
