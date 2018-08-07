using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using XFilesArchive.Model;

namespace XFilesArchive.Services.Lookups
{
    public class FilesOnDriveLookupProvider : ITreeViewLookupProvider<ArchiveEntity>
    {
        private readonly Func<ILookupDataService> _dataServiceCreator;
        private IEnumerable<ArchiveEntity> _frchiveEntityCollectionOnDisk;

        public FilesOnDriveLookupProvider(Func<ILookupDataService> dataServiceCreator)
        {
            _dataServiceCreator = dataServiceCreator;
        }

        public IEnumerable<LookupItemNode> GetLookup()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LookupItemNode> GetLookup(int? DriveId = default(int?))
        {
            var service = _dataServiceCreator();
            
                _frchiveEntityCollectionOnDisk = service.GetAllFilesOnDrive(DriveId ?? 0).ToList();
            

            var ret = _frchiveEntityCollectionOnDisk.Where(x => x.ParentEntityKey == null)
            .OrderBy(l => l.EntityType)
                    .Select(f => new LookupItemNode
                    {
                        Id = f.ArchiveEntityKey,
                        DisplayMember = string.Format("{0}", f.Title),
                        Nodes = GetNodesById(f.ArchiveEntityKey),
                        EntityType = f.EntityType
                    })
                    .ToList();
            return ret;

        }

        public IEnumerable<LookupItemNode> GetLookupWithCondition(Expression<Func<ArchiveEntity, bool>> where
            , Expression<Func<ArchiveEntity, object>> orderby)
        {
            throw new NotImplementedException();
        }

        private ObservableCollection<LookupItemNode> GetNodesById(int archiveEntityKey)
        {

            return new ObservableCollection<LookupItemNode>(
                _frchiveEntityCollectionOnDisk.Where(x => x.ParentEntityKey == archiveEntityKey)
                .OrderBy(l => l.EntityType)
                .Select(
                f => new LookupItemNode
                {
                    Id = f.ArchiveEntityKey,
                    DisplayMember = string.Format("{0}", f.Title),
                    Nodes = GetNodesById(f.ArchiveEntityKey),
                    EntityType = f.EntityType
                }
                ).ToList());

        }


    }
}
