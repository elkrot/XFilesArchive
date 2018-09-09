using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Infrastructure
{
    public  class ConfigurationData: IConfiguration
    {
        public int ThumbnailWidth { get { return 120; }  }
        #region Строка подключения 
        /// <summary>
        /// Строка подключения
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {if(ConfigurationManager.ConnectionStrings["XFilesArchiveDataContext"] !=null)
            return ConfigurationManager.ConnectionStrings["XFilesArchiveDataContext"].ConnectionString;
        else
                return ConfigurationManager.ConnectionStrings["XFilesArchiveDataContext"].ConnectionString;
        }
        #endregion

        #region Название папки Эскиза
        public string GetThumbDirName()
        {
            return "Thumb";
        }
        #endregion

        #region Путь к картинкам
        public string GetTargetImagePath()
        {
            return  Path.Combine(Directory.GetCurrentDirectory(),"img");
        }
        #endregion
    }
}
