using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Search.Condition
{
    public interface ISearchConditionItem
    {
        string Title { get; }
    }

    public class SearchConditionItem : ISearchConditionItem
    {
        private string _title;
        public SearchConditionItem(string title)
        {
            _title = title;
        }
        public string Title { get { return _title; } }
    }
}
