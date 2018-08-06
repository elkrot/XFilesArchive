using LinqSpecs;
using System.Linq;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Widget
{

    #region Item Виджета 
    //public class SearchByTagWidgetItem : ISearchWidgetItem
    //{
    //    public Specification<ArchiveEntity> Specification { get; set; }
    //    public string Title { get; set; }
    //}
    #endregion


    #region  Виджет поиска
    public class SearchByTagWidget : SearchWidget<SearchWidgetItem>
    {
        public void AddQuery(string TagTitle)
        {

            var specification = new AdHocSpecification<ArchiveEntity>(x => x.Tags.Where(t => t.TagTitle == TagTitle).Count() > 0);
            AddItem(new SearchWidgetItem()
            {
                Title = string.Format(@"{0}", TagTitle)
                ,
                Specification = specification
            });
        }




        ///// <summary>

        ///// Adds an equivalent SQL WHERE IN() clause to the query, restricting results to a given range

        ///// </summary>

        ///// <typeparam name="TEntity">Type of entity to query</typeparam>

        ///// <typeparam name="TValue">Type of value to query against</typeparam>

        ///// <param name="query">Existing query</param>

        ///// <param name="selector">Expression to retrieve query field</param>

        ///// <param name="collection">Collection of values to limit query</param>

        ///// <returns>Query with added WHERE IN() clause</returns>

        //public static IQueryable<TEntity> WhereIn<TEntity, TValue>
        //(
        //    this AdHocSpecification<TEntity> query,
        //    Expression<Func<TEntity, TValue>> selector,
        //    IEnumerable<TValue> collection
        //)
        //{
        //    ParameterExpression p = selector.Parameters.Single();
        //   //if there are no elements to the WHERE clause,
        //    //we want no matches:
        //    if (!collection.Any()) return query.Where(x => false);
        //    if (collection.Count() > 3000) //could move this value to config
        //        throw new ArgumentException("Collection too large - execution will cause stack overflow", "collection");
        //    IEnumerable<Expression> equals = collection.Select(value =>
        //       (Expression)Expression.Equal(selector.Body,
        //            Expression.Constant(value, typeof(TValue))));
        //    Expression body = equals.Aggregate((accumulate, equal) =>
        //        Expression.Or(accumulate, equal));
        //    return query.Where(Expression.Lambda<Func<TEntity, bool>>(body, p));

        //}

    }
    #endregion
}
