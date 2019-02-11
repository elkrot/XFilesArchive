using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;

namespace XFilesArchive.Infrastructure
{
    public class DestinationManager
    {
        public MethodResult<List<DestinationItem>> CreateDestinationList(string destinationPath)
        {
            var list =new List<DestinationItem>();


            var result = new MethodResult<List<DestinationItem>>(list);
            try
            {

                FillDestinationItemsList(destinationPath, ref list);
           
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

        private void FillDestinationItemsList(string destinationPath, ref List<DestinationItem> list, Guid parentGuid=default(Guid))
        {
            var directories = Directory.GetDirectories(destinationPath, "*.*", SearchOption.TopDirectoryOnly);
            var files = Directory.GetFiles(destinationPath, "*.*", SearchOption.TopDirectoryOnly);

            var dInfo = new DirectoryInfo(destinationPath);
          
            list.Add(new DestinationItem() { UniqGuid = Guid.NewGuid(),  ParentGuid = parentGuid ,
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


                });
            }

            foreach (var directory in directories)
            {
                FillDestinationItemsList(directory,ref list,parentGuid);
            }

        }
    }
}
