using LinqSpecs;
using System.Linq;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Widget
{
    public class SearchByCategoryWidget : SearchWidget<SearchWidgetItem>
    {
        public void AddQuery(int CategoryKey,string title)
        {

            var specification = new AdHocSpecification<ArchiveEntity>(x => x.Categories.Any(t => t.CategoryKey == CategoryKey));
            AddItem(new SearchWidgetItem()
            {
                Title = string.Format(@"{0}", title)
                ,GroupTitle ="Категории"
                ,
                Specification = specification
            });

        }
    }

}
