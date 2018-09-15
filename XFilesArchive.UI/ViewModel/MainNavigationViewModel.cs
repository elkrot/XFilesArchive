using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XFilesArchive.Infrastructure;
using XFilesArchive.Infrastructure.DataManager;
using XFilesArchive.UI.View;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.ViewModel.Search;

namespace XFilesArchive.UI.ViewModel
{
    public class MainNavigationViewModel:ViewModelBase
    {
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;

        string DriveCode = "";
        string DriveTitle = "";
        string DriveLetter = "";
        int MaxImagesInDirectory = 0;
        byte IsSecret = 0;
        SearchEngineViewModel _searchEngineViewModel;
        private MainViewModel _mainViewModel;

        public MainNavigationViewModel(IEventAggregator eventAggregator
            , IMessageDialogService messageDialogService
            , SearchEngineViewModel searchEngineViewModel
            , MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _searchEngineViewModel = searchEngineViewModel;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            NewDestinationCommand = new DelegateCommand(OnNewDestinationExecute);
            OpenAdminPanelCommand = new DelegateCommand(OnOpenAdminPanelExecute);
            CompareFileCommand = new DelegateCommand(OnCompareFileExecute);
            GoToMainPageCommand = new DelegateCommand(OnGoToMainPageExecute);
            SearchCommand = new DelegateCommand(OnSearchExecute);
        }

        private async void OnGoToMainPageExecute()
        {
            await _mainViewModel.LoadAsync();
            (App.Current.MainWindow as MainWindow).Main.Content = new DrivePage(_mainViewModel);
        }

        private void OnSearchExecute()
        {
            _searchEngineViewModel.Load();
            (App.Current.MainWindow as MainWindow).Main.Content = new SearchPage(_searchEngineViewModel);
        }

        private void OnCompareFileExecute()
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            //myDialog.Filter = "Все файлы (*.*)|Картинки(*.JPG;*.GIF;*.PNG)|*.JPG;*.GIF;*.PNG" + "|*.* ";

            myDialog.Multiselect = false;
            if (myDialog.ShowDialog() == true)
            {
                var cnf = new ConfigurationData();
                var lg = new Logger();
                var fm = new FileManager(cnf, lg);
                FileInfo fi = fm.GetFileInfoByPath(myDialog.FileName);

                var Title = fi.Name;
                var checksum = Infrastructure.Utilites.Security.ComputeMD5Checksum(myDialog.FileName);

                IDataManager dm;

                dm = new DataManager(cnf, fm, lg, 0);
                var result = dm.CheckFilesByHashOrTitle((int)fi.Length, checksum, Title);

                if (result.Count() == 0)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Файл {0} не найден", checksum));
                }
                else
                {
                    var msg = new StringBuilder();
                    msg.AppendLine(string.Format("{0} {1}", checksum, Title));
                    foreach (var item in result)
                    {
                        msg.AppendLine(item);
                    }
                    System.Windows.Forms.MessageBox.Show(msg.ToString());
                }
            }
        }

        private void OnOpenAdminPanelExecute()
        {
            AdminPage page = new AdminPage();
            (App.Current.MainWindow as MainWindow).Main.Navigate(page);
        }

        private void OnNewDestinationExecute()
        {
            ShowWizard();
        }

        public ICommand NewDestinationCommand { get ; }

        public ICommand OpenAdminPanelCommand { get; }

        public ICommand CompareFileCommand { get; }

        public ICommand SearchCommand { get; }
        public ICommand GoToMainPageCommand { get; }

        public async void ShowWizard()
        {

            var _win = new AddDriveWizard();
            _win.DataContext = new WizardData() { DriveCode = "2018_00", DriveLetter = @"e:\", MaxImagesInDirectory = 0 };


            if (_win.ShowDialog() == true)
            {
                var cnf = new ConfigurationData();
                var lg = new Logger();

                try
                {
                    DriveCode = ((WizardData)_win.DataContext).DriveCode;
                    DriveTitle = ((WizardData)_win.DataContext).DriveTitle;

                    DriveLetter = ((WizardData)_win.DataContext).DriveLetter;
                    MaxImagesInDirectory = ((WizardData)_win.DataContext).MaxImagesInDirectory;
                    IsSecret = ((WizardData)_win.DataContext).IsSecret;

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
                          var progress = new Progress<int>(value =>(App.Current.MainWindow as MainWindow).progressBar.Value = value);
                        var id = await worker.Work(progress, token, CreateDestination);
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


            var result = _win.DialogResult.Value;


        }

        private int CreateDestination()
        {
            var cnf = new ConfigurationData() ;
            var lg = new Logger();

            var driveCode = DriveCode;
            var driveTitle = DriveTitle;



            var fm = new FileManager(cnf, lg);

            IDataManager dm = new DataManager(cnf, fm, lg, MaxImagesInDirectory);
            string drvLetter = DriveLetter;
            Dictionary<string, object> addParams = new Dictionary<string, object>();
            addParams.Add("IsSecret", IsSecret);


            int driveId = dm.CreateDrive(DriveLetter, DriveTitle, DriveCode, addParams);
            if (driveId != 0)
            {
                dm.FillDirectoriesInfo(driveId, DriveLetter);
                dm.FillFilesInfo(driveId, DriveLetter);
                dm.ClearCash();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(dm.logger.GetLog());
            }
            return driveId;
        }

    }
}
