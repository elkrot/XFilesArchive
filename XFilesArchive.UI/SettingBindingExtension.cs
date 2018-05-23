using System.Windows.Data;
using XFilesArchive.UI.Properties;

namespace XFilesArchive.UI
{
    public class SettingBindingExtension : Binding
    {
        public SettingBindingExtension()
        {
            Initialize();
        }

        public SettingBindingExtension(string path)
            : base(path)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Source = Settings.Default.Config;
            this.Mode = BindingMode.TwoWay;
        }
    }
}
