using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace XFilesArchive.Services.Lookups
{
    public interface ITreeViewLookupProvider<T>
    {
        IEnumerable<LookupItemNode> GetLookup();

        Task<IEnumerable<LookupItemNode>> GetLookupAsync();

        IEnumerable<LookupItemNode> GetLookup(int? DriveId = default(int?));


        IEnumerable<LookupItemNode> GetLookupWithCondition(Expression<Func<T, bool>> where
            , Expression<Func<T, object>> orderby);
    }
}
