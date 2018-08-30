using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using XFilesArchive.Model;
using XFilesArchive.Search.Condition;
using XFilesArchive.Search.Result;
using XFilesArchive.Search.Widget;
using XFilesArchive.Services.Repositories;
using XFilesArchive.UI.Event;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.ViewModel.Drive;
using XFilesArchive.UI.ViewModel.Navigation;

namespace XFilesArchive.UI.ViewModel.Search
{
    public interface ISearchNavigationViewModel
    {
        void Load();
        SearchResult SearchResult { get; set; }
    }

    public class SearchNavigationViewModel : Observable, ISearchNavigationViewModel
    {

        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private ICategoryNavigationViewModel _categoryNavigationViewModel;
        private ITagNavigationViewModel _tagNavigationViewModel;
        public SearchCondition SearchCondition { get; set; }
        public SearchResult SearchResult { get; set; }
        private IArchiveEntityRepository _repository;
        ICollectionView _viewItems;
        public ICollectionView ViewItems { get { return _viewItems; } }

        public SearchNavigationViewModel(
            IEventAggregator eventAggregator
            , IMessageDialogService messageDialogService
            , ICategoryNavigationViewModel categoryNavigationViewModel
            , ITagNavigationViewModel tagNavigationViewModel
            , IArchiveEntityRepository repository
            )
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _repository = repository;

            _tagNavigationViewModel = tagNavigationViewModel;
            _categoryNavigationViewModel = categoryNavigationViewModel;

            var SearchWidgets = new Dictionary<string, SearchWidget<SearchWidgetItem>>();
            SearchWidgets[nameof(SearchByStringWidget)] = (new SearchByStringWidget());
            SearchWidgets[nameof(SearchByCategoryWidget)] = (new SearchByCategoryWidget());
            SearchWidgets[nameof(SearchByFileSizeWiget)] = (new SearchByFileSizeWiget());
            SearchWidgets[nameof(SearchByTagWidget)] = (new SearchByTagWidget());

            SearchCondition = new SearchCondition(SearchWidgets);

            AddSearchByStringConditionCommand = new DelegateCommand<string>(OnAddSearchByStringConditionExecute, OnAddSearchByStringConditionCanExecute);
            AddSearchByCategoryConditionCommand = new DelegateCommand<int?>(OnAddSearchByCategoryConditionExecute, OnAddSearchByCategoryConditionCanExecute);
            AddSearchByFileSizeConditionCommand = new DelegateCommand<Tuple<string, string>>(OnAddSearchByFileSizeConditionExecute, OnAddSearchByFileSizeConditionCanExecute);
            AddSearchByTagConditionCommand = new DelegateCommand<int?>(OnAddSearchByTagConditionExecute, OnAddSearchByTagConditionCanExecute);
            GoSearchCommand = new DelegateCommand(OnSearchExecute, OnSearchCanExecute);

            ClearConditionCommand = new DelegateCommand(OnClearConditionExecute, OnClearConditionCanExecute);
            _viewItems = (CollectionView)CollectionViewSource.GetDefaultView(SearchCondition.Items);
            //TODO: комманду очистить условие


            PropertyGroupDescription groupDescription = new PropertyGroupDescription("GroupTitle");
            _viewItems.GroupDescriptions.Add(groupDescription);
        }

        private bool OnSearchCanExecute()
        {

            return true;
        }

        private void OnSearchExecute()
        {
            var condition = SearchCondition.Condition;
            var searchItems = _repository.GetEntitiesByCondition(condition);
            SearchResult = new SearchResult(searchItems);

            _eventAggregator.GetEvent<ShowSearchResultEvent>().Publish(0);
        }

        private bool OnAddSearchByTagConditionCanExecute(int? obj)
        {
            return true;
        }

        private void OnAddSearchByTagConditionExecute(int? tagKey)
        {
            var tag = _repository.GetTagByKey((int)tagKey);
            //obj as HomeArchiveX.WpfUI.ViewModel.FilesOnDrive.NavigationTagItemViewModel;

            if (tag != null)
            {
                if (SearchCondition.Widgets.ContainsKey(nameof(SearchByTagWidget)))
                {
                    (SearchCondition.Widgets[nameof(SearchByTagWidget)] as SearchByTagWidget).AddQuery(tag.TagTitle);
                    SearchCondition.LoadItems();
                }
            }
        }

        private bool OnAddSearchByFileSizeConditionCanExecute(Tuple<string, string> fileSize)
        {
            return true;
        }

        private void OnAddSearchByFileSizeConditionExecute(Tuple<string, string> fileSize)
        {
            int minFileSize = int.Parse(fileSize.Item1);
            int maxFileSize = int.Parse(fileSize.Item2);

            if (SearchCondition.Widgets.ContainsKey(nameof(SearchByFileSizeWiget)))
            {
                (SearchCondition.Widgets[nameof(SearchByFileSizeWiget)] as SearchByFileSizeWiget).AddQuery(minFileSize, maxFileSize);
                SearchCondition.LoadItems();
                _viewItems = (CollectionView)CollectionViewSource.GetDefaultView(SearchCondition.Items.ToList());

            }
        }

        private bool OnAddSearchByCategoryConditionCanExecute(int? obj)
        {
            return true;
        }

        private void OnAddSearchByCategoryConditionExecute(int? categoryId)
        {
            if (categoryId != null && categoryId > 0)
            {
                int categoryKey = (int)categoryId;
                if (categoryKey > 0)
                {
                    var category = _repository.GetCategoryById(categoryKey);

                    if (SearchCondition.Widgets.ContainsKey(nameof(SearchByCategoryWidget)))
                    {
                        (SearchCondition.Widgets[nameof(SearchByCategoryWidget)] as SearchByCategoryWidget).AddQuery(categoryKey, category.CategoryTitle);
                        SearchCondition.LoadItems();
                        _viewItems = (CollectionView)CollectionViewSource.GetDefaultView(SearchCondition.Items);
                    }
                }
            }

        }

        private bool OnAddSearchByStringConditionCanExecute(string obj)
        {
            return true;
        }

        private void OnAddSearchByStringConditionExecute(object obj)
        {
            if (!string.IsNullOrWhiteSpace(obj.ToString()))
            {
                if (SearchCondition.Widgets.ContainsKey(nameof(SearchByStringWidget)))
                {
                    string searchString = obj.ToString();
                    (SearchCondition.Widgets[nameof(SearchByStringWidget)] as SearchByStringWidget).AddQuery(searchString);
                    SearchCondition.LoadItems();
                    _viewItems = (CollectionView)CollectionViewSource.GetDefaultView(SearchCondition.Items);
                }
            }
            //            _messageDialogService.ShowMessageDialog("", obj.ToString());
        }

        private void OnClearConditionExecute()
        {
            SearchCondition.ClearItems();
        }

        private bool OnClearConditionCanExecute()
        {
            return true;
        }

        public ICommand AddSearchByStringConditionCommand { get; private set; }
        public ICommand AddSearchByCategoryConditionCommand { get; private set; }
        public ICommand AddSearchByFileSizeConditionCommand { get; private set; }
        public ICommand AddSearchByTagConditionCommand { get; private set; }
        public ICommand ClearConditionCommand { get; private set; }
        public ICommand GoSearchCommand { get; private set; }

        
        public ICategoryNavigationViewModel CategoryNavigationViewModel
        {
            get
            {
                return _categoryNavigationViewModel;
            }
        }

        public ITagNavigationViewModel TagNavigationViewModel
        {
            get
            {
                return _tagNavigationViewModel;
            }
        }

        public void Load()
        {
            CategoryNavigationViewModel.Load();
            TagNavigationViewModel.Load();
        }
    }
}
