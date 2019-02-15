using HomeArchiveX.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Infrastructure.DataManager;
using XFilesArchive.Model;

namespace XFilesArchive.Infrastructure
{
    public class DestinationManager
    {
        private FillInfoParameters _imgInfoParams;
        private IDataManager _dm;
        private IFIleManager _fm;
        private ConfigurationData _cnf;


        public DestinationManager( FillInfoParameters imgInfoParams
            ,IDataManager dm, IFIleManager fm, ConfigurationData cnf)
        {
            _imgInfoParams = imgInfoParams;
            _dm = dm;
            _fm = fm;
            _cnf = cnf;
        }

        #region CreateDestinationList
    public MethodResult<List<DestinationItem>> CreateDestinationList()
        {
            var list =new List<DestinationItem>();
            var result = new MethodResult<List<DestinationItem>>(list);
            try
            {
                FillDestinationItemsList(_imgInfoParams.DestinationPath, ref list);
            }
            catch (UnauthorizedAccessException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            catch (FileNotFoundException e)
            {
                result.Success = false;
                result.Messages.Add(e.Message);
            }
            return result;
        }
        #endregion
    
        #region FillDestinationItemsList
       private void FillDestinationItemsList(string destinationPath, ref List<DestinationItem> list, Guid parentGuid=default(Guid))
        {
            var directories = Directory.GetDirectories(destinationPath, "*.*", SearchOption.TopDirectoryOnly);
            var files = Directory.GetFiles(destinationPath, "*.*", SearchOption.TopDirectoryOnly);

            var dInfo = new DirectoryInfo(destinationPath);

            var newDirectoryGuid = Guid.NewGuid();
            list.Add(new DestinationItem() { UniqGuid = newDirectoryGuid,  ParentGuid = parentGuid ,
            EntityPath=dInfo.Root.Name ,EntityType = 1, Title = dInfo.FullName
            });

            foreach (var file in files)
            {
                var fInfo = new FileInfo(file);
                list.Add(new DestinationItem() { UniqGuid = Guid.NewGuid(), ParentGuid = parentGuid 
                    , EntityExtension = fInfo.Extension
                    , EntityType = 2
                    , EntityPath = fInfo.DirectoryName
                    , Title = fInfo.FullName
                    , FileSize = fInfo.Length
                    , Checksum = Utilites.Security.ComputeMD5Checksum(fInfo.FullName)

            });
            }

            foreach (var directory in directories)
            {
                FillDestinationItemsList(directory,ref list, newDirectoryGuid);
            }

        }



        #endregion

        #region Execute
        public void Execute()
        {
            var result = CreateDestinationList();

            _dm.BulkCopyArchiveEntity(_cnf.GetConnectionString(), result.Result, _imgInfoParams.DriveId);

            var dest = new Destination(_imgInfoParams.DriveId, result.Result);

            if (_imgInfoParams.FillImages)
            {
                dest.FillImageInfo();
            }
            if (_imgInfoParams.FillMedia)
            {
                dest.FillMediaInfo();
            }
        }
        #endregion

    }
}
