using LinqSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Search.Widget
{
    public class SearchByGradeWidget : SearchWidget<SearchWidgetItem>
    {
        public void AddQuery(int? minGrade,int? maxGrade)
        {
            var specification = new AdHocSpecification<ArchiveEntity>(x =>x.Grade>=minGrade && x.Grade<=maxGrade);
            AddItem(new SearchWidgetItem()
            {
                Title = string.Format(@"{0}-{1}",minGrade, maxGrade)
                ,
                GroupTitle = "Оценка"
                ,
                Specification = specification
            });
        }
    }
}
