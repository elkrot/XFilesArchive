﻿using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using XFilesArchive.Model;
using XFilesArchive.Services.Lookups;
using System;
using System.Threading.Tasks;

namespace XFilesArchive.UI.ViewModel.Drive
{
    public interface ICategoryNavigationViewModel
    {
        void Load();
        Task LoadAsync();
    }


    public class CategoryNavigationViewModel : ICategoryNavigationViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ITreeViewLookupProvider<Category> _categoryLookupProvider;

        public CategoryNavigationViewModel(IEventAggregator eventAggregator,
          ITreeViewLookupProvider<Category> categoryLookupProvider)
        {
            _eventAggregator = eventAggregator;
            _categoryLookupProvider = categoryLookupProvider;
            NavigationItems = new ObservableCollection<NavigationCategoryTreeItemViewModel>();
        }

        public void Load()
        {
            IEnumerable<LookupItemNode> items;

            items = _categoryLookupProvider.GetLookup();

            NavigationItems.Clear();

            foreach (var categoryLookupItem in
                items)
            {
                NavigationItems.Add(
                  new NavigationCategoryTreeItemViewModel(
                    categoryLookupItem,
                    _eventAggregator));
            }
        }

        public async Task LoadAsync()
        {
            IEnumerable<LookupItemNode> items;

            items = await _categoryLookupProvider.GetLookupAsync();

            NavigationItems.Clear();

            foreach (var categoryLookupItem in
                items)
            {
                NavigationItems.Add(
                  new NavigationCategoryTreeItemViewModel(
                    categoryLookupItem,
                    _eventAggregator));
            }
        }

        public ObservableCollection<NavigationCategoryTreeItemViewModel> NavigationItems { get; set; }
    }


    //------------------------------
    public class NavigationCategoryTreeItemViewModel : Observable
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayValue;

        #region Constructor
        public NavigationCategoryTreeItemViewModel(LookupItemNode itemNode,
          IEventAggregator eventAggregator)
        {
            CategoryKey = itemNode.Id;
            DisplayValue = itemNode.DisplayMember;
            NavigationItems = GetItems(itemNode.Nodes);
            _eventAggregator = eventAggregator;
            //OpenFileOnDriveEditViewCommand = new DelegateCommand(OpenFileOnDriveEditViewExecute);
        }

        private ObservableCollection<NavigationCategoryTreeItemViewModel> GetItems(ObservableCollection<LookupItemNode> nodes)
        {
            var retItems = new ObservableCollection<NavigationCategoryTreeItemViewModel>();
            foreach (var item in nodes)
            {
                var itm = new NavigationCategoryTreeItemViewModel(
                    item,
                    _eventAggregator);
                retItems.Add(itm);

            }
            return retItems;
        }
        #endregion


        public int CategoryKey { get; private set; }
        public ObservableCollection<NavigationCategoryTreeItemViewModel> NavigationItems { get; set; }
        public string ImagePath { get; set; }
        #region DisplayValue
        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value.Trim();
                OnPropertyChanged();
            }
        }
        #endregion



    }
}
