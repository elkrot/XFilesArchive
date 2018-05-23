
using Autofac;
using System;
using System.Windows;
using XFilesArchive.UI.Properties;
using XFilesArchive.UI.Startup;

namespace XFilesArchive.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured. Please inform the admin." +
               Environment.NewLine + e.Exception.Message, "UnexpectedError");
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
