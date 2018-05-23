using System;
using System.Configuration;

namespace XFilesArchive.Infrastructure
{
    public class ConfigManager
    {
        private static ConfigManager instance;

        private Configuration configFile;
        private KeyValueConfigurationCollection settings;

        #region Свойства Конфигурации
        public DateTime StartDate
        {
            get
            {
                DateTime result;
                DateTime.TryParse(GetSetting(nameof(StartDate)), out result);
                return result;
            }
            set { AddUpdateAppSettings(nameof(StartDate), value.ToShortDateString()); }
        }
        public DateTime EndDate
        {
            get
            {
                DateTime result;
                DateTime.TryParse(GetSetting(nameof(EndDate)), out result);
                return result;
            }
            set { AddUpdateAppSettings(nameof(EndDate), value.ToShortDateString()); }
        }

        #endregion

        #region Описание класса
        private ConfigManager()
        {
            configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings = configFile.AppSettings.Settings;
        }

        public static ConfigManager getInstance()
        {
            if (instance == null)
                instance = new ConfigManager();
            return instance;
        }



        private string GetSetting(string key)
        {
            try
            {
                return settings[key] == null ? "" : settings[key].Value ?? "";

            }
            catch (ConfigurationErrorsException)
            {
                return "";
            }
        }

        private void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {

            }
        }
        #endregion



    }



    public class StartupFoldersConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Folders")]
        public FoldersCollection FolderItems
        {
            get { return ((FoldersCollection)(base["Folders"])); }
        }
    }



    [ConfigurationCollection(typeof(FolderElement), AddItemName = "Folder")]
    public class FoldersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderElement)(element)).FolderType;
        }

        public FolderElement this[int idx]
        {
            get { return (FolderElement)BaseGet(idx); }
        }
    }

    public class FolderElement : ConfigurationElement
    {

        [ConfigurationProperty("folderType", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string FolderType
        {
            get { return ((string)(base["folderType"])); }
            set { base["folderType"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Path
        {
            get { return ((string)(base["path"])); }
            set { base["path"] = value; }
        }
    }





}
