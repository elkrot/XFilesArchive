using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XFilesArchive.Model;
using XFilesArchive.Services.Lookups;

namespace XFilesArchive.Search.Result
{
    public class SearchResult : DependencyObject, ISearchResult
    {
        public ObservableCollection<ISearchResultItem> Items
        { get; set; }

        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }


        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(SearchResult), new PropertyMetadata(0));


        public SearchResult(ICollection<ArchiveEntityDto> items)
        {
            Items = new ObservableCollection<ISearchResultItem>();
            foreach (var item in items)
            {
                Items.Add(new SearchResultItem(item));
            }
        }
    }
}
