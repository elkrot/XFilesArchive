using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace XFilesArchive.UI.Services.Lookups
{
    public interface ITreeViewLookupProvider<T>
    {
        IEnumerable<LookupItemNode> GetLookup();
        IEnumerable<LookupItemNode> GetLookup(int? DriveId = default(int?));


        IEnumerable<LookupItemNode> GetLookupWithCondition(Expression<Func<T, bool>> where
            , Expression<Func<T, object>> orderby);
    }
}
