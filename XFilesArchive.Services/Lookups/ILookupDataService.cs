using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Lookups
{
    public interface ILookupDataService
    {
        Task<IEnumerable<LookupItem>> GetDriveLookupAsync();
        Task<IEnumerable<Drive>> GetDrivesByConditionAsync(
           Expression<Func<Drive, bool>> where, Expression<Func<Drive, object>> orderby);

        Task<IEnumerable<DriveDto>> GetDrivesByConditionAsync(Expression<Func<Drive, bool>> where
            , Expression<Func<Drive, object>> orderby
            , bool isDescending, int index, int length);




        Task<int> GetDrivesCountByConditionAsync(Expression<Func<Drive, bool>> where
    , Expression<Func<Drive, object>> orderby
    , bool isDescending, int index, int length);



        IEnumerable<DriveDto> GetDrivesByCondition(Expression<Func<Drive, bool>> where
    , Expression<Func<Drive, object>> orderby
    , bool isDescending, int index, int length);




        IEnumerable<ArchiveEntity> GetAllFilesOnDrive(int id);

        int GetDrivesCountByCondition(Expression<Func<Drive, bool>> where
    , Expression<Func<Drive, object>> orderby
    , bool isDescending, int index, int length);


    }
}