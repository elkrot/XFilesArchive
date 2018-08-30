using LinqSpecs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows;
using XFilesArchive.Model;
using XFilesArchive.Search.Widget;
using System.Windows.Data;
using System.ComponentModel;

namespace XFilesArchive.Search.Condition
{
    public class SearchCondition : DependencyObject, ISearchCondition
    {
        ObservableCollection<ISearchConditionItem> _items;

        public ObservableCollection<ISearchConditionItem> Items
        {
            get { return _items; }
        }

        public void LoadItems()
        {
            _items.Clear();
            foreach (var wiget in Widgets)
            {
                foreach (var item in wiget.Value.Items)
                {
                    _items.Add(new SearchConditionItem(item.Title,item.GroupTitle));
                }
            }
        }
        public void ClearItems()
        {
            _items.Clear();
            _widgets.Clear();
                  

        }
        private Dictionary<string, SearchWidget<SearchWidgetItem>> _widgets;

        public SearchCondition(Dictionary<string, SearchWidget<SearchWidgetItem>> widgets)
        {
            _items = new ObservableCollection<ISearchConditionItem>();
            _widgets = widgets;

        }

        public Dictionary<string, SearchWidget<SearchWidgetItem>> Widgets
        {
            get { return _widgets; }
            set { _widgets = value; }
        }

        public Expression<Func<ArchiveEntity, bool>> Condition
        {
            get
            {
                Specification<ArchiveEntity> specification = new AdHocSpecification<ArchiveEntity>(n => n.ArchiveEntityKey == n.ArchiveEntityKey);
                Expression<Func<ArchiveEntity, bool>> Result;
                foreach (var wiget in _widgets)
                {
                    foreach (var item in wiget.Value.Items)
                    {
                        specification = new AndSpecification<ArchiveEntity>(specification, item.Specification);
                    }
                }
                Result = specification.IsSatisfiedBy();

                var expression = Result.Compile();

                return Result;
            }
        }

     
    }
}
