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
        string GroupTitle { get; }
    }

    public class SearchConditionItem : ISearchConditionItem
    {
        private string _title;
        private string _groupTitle;

        public SearchConditionItem(string title,string groupTitle)
        {
            _title = title;
            _groupTitle = groupTitle;
        }
        public string Title { get { return _title; } }

        public string GroupTitle { get { return _groupTitle; } }
    }
}
