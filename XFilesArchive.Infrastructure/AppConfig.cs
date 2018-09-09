using System;
using System.ComponentModel;
using System.Windows;

namespace XFilesArchive.UI
{
    [Serializable]
    public class AppConfig
    {
        public WindowConfig MainWindow { get; set; }
        public Theme Theme { get; set; }

        [Category("Главное окно")]
        [DisplayName("Width")]
        [Description("Шприна Главного окна.")]
        public int MainWindowWidth { get { return MainWindow.Width; } set { MainWindow.Width = value; } }

        public AppConfig()
        {
            MainWindow = new WindowConfig();
            MainWindow.Width = 600;
            MainWindow.Height = 400;

            Theme = Theme.Light;
        }
    }

    [Serializable]
    public class WindowConfig
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public WindowState WindowState { get; set; }

        public WindowConfig()
        {
        }
    }
    public enum Theme
    {
        Light,
        Dark,
        Blue
    }
}
