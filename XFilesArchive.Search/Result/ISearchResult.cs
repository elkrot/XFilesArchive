using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Search.Result
{
    public interface ISearchResult
    {
        ObservableCollection<ISearchResultItem> Items { get; set; }
    }
}
