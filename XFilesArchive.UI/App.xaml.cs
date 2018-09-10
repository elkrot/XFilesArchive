
using Autofac;
using System;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows;
using XFilesArchive.Infrastructure;
using XFilesArchive.UI.Properties;
using XFilesArchive.UI.Startup;

namespace XFilesArchive.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        ResourceManager LocRM;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            LocRM = new ResourceManager("XFilesArchive.UI.Resources", typeof(MainWindow).Assembly);
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(LocRM.GetString("UnexpectedError") +
               Environment.NewLine + e.Exception.Message + Environment.NewLine +
               e.Exception.Source + Environment.NewLine + e.Exception.StackTrace
               , "UnexpectedError");
            e.Handled = true;




        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var commandLineArgs = System.Environment.GetCommandLineArgs();
            if (Settings.Default.Config == null)
            {
                Settings.Default.Config = new AppConfig();
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            var commandLineArgs = System.Environment.GetCommandLineArgs();
            Settings.Default.Save();
        }
    }
}
