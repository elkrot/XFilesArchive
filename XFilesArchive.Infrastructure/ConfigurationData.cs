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
        public ConfigurationData():this("XFilesArchiveDataContext")
        {

        }
        AppConfig _appConfig;
        public ConfigurationData(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public ConfigurationData(string connectionStringName, AppConfig appConfig):this(connectionStringName)
        {
            _connectionStringName = connectionStringName;
            if (appConfig == null)
            {
                _appConfig = new AppConfig();
            }
            else
            {
                _appConfig = appConfig;
            }
        }

        private string _connectionStringName;

        public int ThumbnailWidth { get { return _appConfig.ThumbnailWidth??120; }  }
        #region Строка подключения 
        /// <summary>
        /// Строка подключения
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {if(ConfigurationManager.ConnectionStrings[_connectionStringName] !=null)
            return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
        else
            return ConfigurationManager.ConnectionStrings[0].ConnectionString;
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
