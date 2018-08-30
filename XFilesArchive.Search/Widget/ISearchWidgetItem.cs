using LinqSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Widget
{
    public interface ISearchWidgetItem
    {
        string Title { get; set; }
        string GroupTitle { get; set; }
        Specification<ArchiveEntity> Specification { get; set; }
    }

    public class SearchWidgetItem : ISearchWidgetItem
    {
        public Specification<ArchiveEntity> Specification { get; set; }
        public string Title { get; set; }
        public string GroupTitle { get; set; }
    }
}
