using LinqSpecs;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Widget
{

    #region Item Виджета 
    //public class SearchByFileSizeWidgetItem : ISearchWidgetItem
    //{
    //    public Specification<ArchiveEntity> Specification { get; set; }
    //    public string Title { get; set; }
    //}
    #endregion


    public class SearchByFileSizeWiget : SearchWidget<SearchWidgetItem>
    {
        public void AddQuery(int Filesize)
        {
            var specification = new AdHocSpecification<ArchiveEntity>(x => x.FileSize == Filesize);
            AddItem(new SearchWidgetItem() { Title = string.Format("Размер равен {0}", Filesize), Specification = specification });
        }

        public void AddQuery(int minFilesize, int maxFilesize)
        {
            var spMin = new AdHocSpecification<ArchiveEntity>(x => x.FileSize >= minFilesize);
            var spMax = new AdHocSpecification<ArchiveEntity>(x => x.FileSize <= maxFilesize);
            var specification = new AndSpecification<ArchiveEntity>(spMin, spMax);
            AddItem(new SearchWidgetItem()
            {
                Title = string.Format("Размер между {0} и {1}", minFilesize, maxFilesize)
                ,
                GroupTitle = "Размер файла"
                ,
                Specification = specification
            });
        }

    }
}
