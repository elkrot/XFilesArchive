using LinqSpecs;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Widget
{
    #region Item Виджета 
    //public class SearchByStringWidgetItem : ISearchWidgetItem
    //{
    //    public string Title { get; set; }
    //    public Specification<ArchiveEntity> Specification { get; set; }
    //}
    #endregion


    #region  Виджет поиска
    public class SearchByStringWidget : SearchWidget<SearchWidgetItem>
    {
        public void AddQuery(string searchString)
        {
            //   var str1 = new AdHocSpecification<ArchiveEntity>(x => x.Title.Contains(searchString));
            //   var str2 = new AdHocSpecification<ArchiveEntity>(x => x.Description.Contains(searchString));
            var str1 = new AdHocSpecification<ArchiveEntity>(x => x.Title.Contains("searchString"));
            var str2 = new AdHocSpecification<ArchiveEntity>(x => x.Description.Contains("searchString"));



            //var specification =new OrSpecification<ArchiveEntity>(str1, str2);

            var specification = new AdHocSpecification<ArchiveEntity>(x => x.Title.Contains(searchString));
            AddItem(new SearchWidgetItem()
            {
                Title = string.Format(@"'{0}'", searchString)
                ,
                Specification = specification
            });
        }
    }
    #endregion
}
