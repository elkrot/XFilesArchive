﻿using Autofac;
using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Windows;
using XFilesArchive.Infrastructure;
using XFilesArchive.Security;
using XFilesArchive.UI.Properties;
using XFilesArchive.UI.Startup;
using XFilesArchive.UI.View.Security;
using XFilesArchive.UI.ViewModel.Security;

namespace XFilesArchive.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        ResourceManager LocRM;
        IAppLogger appLoger;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            LocRM = new ResourceManager("XFilesArchive.UI.Resources", typeof(LoginWindow).Assembly);
            appLoger = new AppLogger("XFileArchive");
            //var bootstrapper = new Bootstrapper();
            //var container = bootstrapper.Bootstrap();
            //var mainWindow = container.Resolve<MainWindow>();
            //mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            //System.Security.SecurityException
            if (e.Exception.GetType().Name == "SecurityException") {
                appLoger.SetLog("Отказано в доступе!", System.Diagnostics.EventLogEntryType.Information);
                MessageBox.Show("Отказано в доступе!");
                e.Handled = true;
                return;
            }

            //" LocRM.GetString("UnexpectedError")
            var msg = "Неопознаная ошибка"+
               Environment.NewLine + e.Exception.Message + Environment.NewLine +
               e.Exception.Source + Environment.NewLine + e.Exception.StackTrace + e.Exception.GetType().Name;

            appLoger.SetLog(msg, System.Diagnostics.EventLogEntryType.Information);

            MessageBox.Show(msg, "UnexpectedError");
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
            var pathToMdfFileDirectory = Directory.GetCurrentDirectory();// @"d:\temp\";
            AppDomain.CurrentDomain.SetData("DataDirectory", pathToMdfFileDirectory);


            try
            {
                //Create a custom principal with an anonymous identity at startup
                CustomPrincipal customPrincipal = new CustomPrincipal(new AnonymousIdentity());
                AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

                base.OnStartup(e);

                //Show the login view
                AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
                LoginWindow loginWindow = new LoginWindow(viewModel);
                loginWindow.Show();
                base.OnStartup(e);

            }
            catch (Exception ex)
            {
                appLoger.SetLog(ex.Message+ ex.StackTrace, System.Diagnostics.EventLogEntryType.Information);
                System.Windows.Forms.MessageBox.Show(ex.Message);

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