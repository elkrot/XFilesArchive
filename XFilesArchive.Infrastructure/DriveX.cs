using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Infrastructure
{
    #region Drive
    public class DriveX
    {
        public int id;
        public string title;
        public DriveInfo driveInfo;
    }
    #endregion

    #region Тип сущности
    public enum EntityType
    {
        Folder = 1
        , File = 2
    }
    #endregion
}
