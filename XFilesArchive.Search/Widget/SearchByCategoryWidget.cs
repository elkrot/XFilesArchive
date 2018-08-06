using LinqSpecs;
using System.Linq;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Widget
{
    public class SearchByCategoryWidget : SearchWidget<SearchWidgetItem>
    {
        public void AddQuery(int CategoryKey)
        {

            var specification = new AdHocSpecification<ArchiveEntity>(x => x.Categories.Where(t => t.CategoryKey == CategoryKey).Count() > 0);
            AddItem(new SearchWidgetItem()
            {
                Title = string.Format(@"{0}", CategoryKey)
                ,
                Specification = specification
            });

        }
    }

}
