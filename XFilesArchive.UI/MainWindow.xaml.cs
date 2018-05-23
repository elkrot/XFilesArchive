using MahApps.Metro.Controls;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using XFilesArchive.Infrastructure;
using XFilesArchive.UI.View;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow :  MetroWindow
    {    
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;

        string DriveCode = "";
        string DriveTitle = "";
        string DriveLetter = "";
        int MaxImagesInDirectory = 0;
        byte IsSecret = 0;
        private MetroWindow _window;


        public MainWindow(MainViewModel viewModel)
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            InitializeComponent();
            Main.Content = new DrivePage(viewModel);
        }




        void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ShowWizard();
            System.Diagnostics.Process.Start("notepad.exe");
        }

        public void ShowWizard() {
            Xceed.Wpf.Toolkit.Wizard wizard = this.Resources["_wizard"] as Xceed.Wpf.Toolkit.Wizard;
            if (wizard != null)
            {
                wizard.CurrentPage = wizard.Items[0] as Xceed.Wpf.Toolkit.WizardPage;

                if (_window != null)
                {
                    _window.Content = null;
                    _window = null;
                }
                _window = new MetroWindow();
                _window.Title = "Создание Описания Файлов для дирректории.";
                _window.Content = wizard;
                _window.DataContext = new WizardData() { DriveCode = "2018_00", DriveLetter = @"e:\", MaxImagesInDirectory = 0 };
                _window.Width = 600;
                _window.Height = 400;
                _window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                // Window will be closed by Wizard because FinishButtonClosesWindow = true and CancelButtonClosesWindow = true

                if (_window.ShowDialog() == true)
                {
                    var cnf = new ConfigurationData();
                    var lg = new Logger();

                    try
                    {
                        DriveCode = ((WizardData)_window.DataContext).DriveCode;
                        DriveTitle = ((WizardData)_window.DataContext).DriveTitle;

                        DriveLetter = ((WizardData)_window.DataContext).DriveLetter;
                        MaxImagesInDirectory = ((WizardData)_window.DataContext).MaxImagesInDirectory;
                        IsSecret = ((WizardData)_window.DataContext).IsSecret;

                        //var fm = new FileManager(cnf, lg);

                        //IDataManager dm = new DataManager(cnf, fm, lg, MaxImagesInDirectory);
                        //string drvLetter = DriveLetter;
                        //Dictionary<string, object> addParams = new Dictionary<string, object>();
                        //addParams.Add("IsSecret", IsSecret);

                        // CrtDrv(dm, drvLetter, DriveTitle, DriveCode, addParams);

                        //-----------
                        var worker = new Worker();

                        cancelTokenSource = new CancellationTokenSource();
                        token = cancelTokenSource.Token;
                  //      var progress = new Progress<int>(value => progressBar.Value = value);
                    //    var id = await worker.Work(progress, token, CreateDestination);
                        //------------


                       // _drivesViewModel.Load();
                        System.Windows.Forms.MessageBox.Show("Обработка Завершена");
                      //  progressBar.Value = 0;
                        var Log = lg.GetLog();
                        if (!string.IsNullOrWhiteSpace(Log))
                        {
                            System.Windows.Forms.MessageBox.Show(Log);
                        }
                    }
                    catch (Exception er)
                    {

                        System.Windows.Forms.MessageBox.Show(er.Message);
                        if (!string.IsNullOrWhiteSpace(lg.GetLog()))
                        {
                            System.Windows.Forms.MessageBox.Show(lg.GetLog());
                        }

                    }

                }


            }
        }
    }
}
