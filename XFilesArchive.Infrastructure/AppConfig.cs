using System;
using System.ComponentModel;
using System.Windows;

namespace XFilesArchive.Infrastructure
{
    [Serializable]
    [DisplayName("Конфигурация приложения")]
    public class AppConfig
    {
        public WindowConfig MainWindow { get; set; }
        public Theme Theme { get; set; }

        [Category("Главное окно")]
        [DisplayName("Ширина")]
        [Description("Ширина Главного окна.")]
        public int MainWindowWidth { get { return MainWindow.Width; }  set { MainWindow.Width = value; } }

        [Category("Сохранение")]
        [DisplayName("Ширина Эскиза")]
        [Description("Ширина Эскиза.")]
        public int? ThumbnailWidth
        {
            get;set;
        }

        [Category("Сохранение")]
        [DisplayName("Высота Эскиза")]
        [Description("Высота Эскиза.")]
        public int? ThumbnailHeight
        {
            get; set;
        }

        public AppConfig()
        {
            MainWindow = new WindowConfig();
            MainWindow.Width = 600;
            MainWindow.Height = 400;
            ThumbnailWidth = 120;
            ThumbnailHeight = 120;
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
