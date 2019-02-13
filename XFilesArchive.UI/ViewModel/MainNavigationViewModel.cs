using Microsoft.Win32;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Input;
using XFilesArchive.Infrastructure;
using XFilesArchive.Infrastructure.DataManager;
using XFilesArchive.Model;
using XFilesArchive.UI.View;
using XFilesArchive.UI.View.Services;
using XFilesArchive.UI.ViewModel.Search;

namespace XFilesArchive.UI.ViewModel
{
    public class MainNavigationViewModel : ViewModelBase
    {
        #region Fields
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;
        string DriveCode = "";
        string DriveTitle = "";
        string DriveLetter = "";
        int MaxImagesInDirectory = 0;
        byte IsSecret = 0;
        SearchEngineViewModel _searchEngineViewModel;
        private MainViewModel _mainViewModel;
        private IMessageDialogService _messageDialogService;
        #endregion

        #region MainNavigationViewModel Constructor
        public MainNavigationViewModel(IEventAggregator eventAggregator
               , IMessageDialogService messageDialogService
               , SearchEngineViewModel searchEngineViewModel
               , MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _searchEngineViewModel = searchEngineViewModel;
            _messageDialogService = messageDialogService;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            NewDestinationCommand = new Prism.Commands.DelegateCommand(OnNewDestinationExecute);

            NewDestinationCommandX = new Prism.Commands.DelegateCommand(OnNewDestinationExecuteX);

            OpenAdminPanelCommand = new Prism.Commands.DelegateCommand(OnOpenAdminPanelExecute);
            CompareFileCommand = new Prism.Commands.DelegateCommand(OnCompareFileExecute);
            GoToMainPageCommand = new Prism.Commands.DelegateCommand(OnGoToMainPageExecute);
            SearchCommand = new Prism.Commands.DelegateCommand(OnSearchExecute);
            //PrincipalPermission principalPerm = new PrincipalPermission(null, "Administrator");
            //CustomPrincipal wp = Thread.CurrentPrincipal as CustomPrincipal;
            //if (wp != null)
            //    if (wp.IsInRole(@"administrator") == true)
            //    {
            //        //  messagebox.show("accessed");

            //    }
            //        Debug.WriteLine("accessed");
            // MessageBox.Show(principalPerm.ToString());

        }

        #endregion

        #region OnGoToMainPageExecute
        private async void OnGoToMainPageExecute()
        {
            //await _mainViewModel.LoadAsync();
            _mainViewModel.Load();
            if (App.Current.Windows.OfType<MainWindow>().FirstOrDefault() is MainWindow)
            {
                App.Current.Windows.OfType<MainWindow>().FirstOrDefault().Main.Content = new DrivePage(_mainViewModel);
            }
            else
            {
                await _messageDialogService.ShowInfoDialogAsync("Ошибка");
            }
        }

        #endregion

        #region OnSearchExecute
        private async void OnSearchExecute()
        {
            _searchEngineViewModel.Load();
            if (App.Current.Windows.OfType<MainWindow>().FirstOrDefault() is MainWindow)
            {

                (App.Current.Windows.OfType<MainWindow>().FirstOrDefault() as MainWindow).Main.Content = new SearchPage(_searchEngineViewModel);
            }
            else
            {
                await _messageDialogService.ShowInfoDialogAsync("Ошибка");
            }
        }

        #endregion

        #region OnCompareFileExecute
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

        #endregion

        #region OnOpenAdminPanelExecute
        [PrincipalPermission(SecurityAction.Demand, Role = "Administrator")]
        private async void OnOpenAdminPanelExecute()
        {
            AdminPage page = new AdminPage();
            if (App.Current.Windows.OfType<MainWindow>().FirstOrDefault() is MainWindow)
            {

                (App.Current.Windows.OfType<MainWindow>().FirstOrDefault() as MainWindow).Main.Navigate(page);
            }
            else
            {
                await _messageDialogService.ShowInfoDialogAsync("Ошибка");
            }
        }
        #endregion

        #region OnNewDestinationExecute
        private void OnNewDestinationExecute()
        {
            ShowWizard();
        }
        #endregion

        #region OnNewDestinationExecuteX
        private void OnNewDestinationExecuteX()
        {
            ShowWizardX();
        }
        #endregion

        #region ShowWizardX
        private async void ShowWizardX()
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
                    var worker = new Worker();
                    cancelTokenSource = new CancellationTokenSource();
                    token = cancelTokenSource.Token;
                    if (App.Current.Windows.OfType<MainWindow>().FirstOrDefault() is MainWindow)
                    {
                        System.Windows.Forms.MessageBox.Show("Обработка Начата");
                        var progress = new Progress<int>(value => (App.Current.Windows.OfType<MainWindow>().FirstOrDefault()).progressBar.Value = value);
                        var id = await worker.Work(progress, token, CreateDestinationX);
                        var Log = lg.GetLog();
                        if (!string.IsNullOrWhiteSpace(Log))
                        {
                            System.Windows.Forms.MessageBox.Show(Log);
                        }
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

        #endregion

        #region Commands
        public ICommand NewDestinationCommand { get; }
        public ICommand NewDestinationCommandX { get; }

        public ICommand OpenAdminPanelCommand { get; }

        public ICommand CompareFileCommand { get; }

        public ICommand SearchCommand { get; }
        public ICommand GoToMainPageCommand { get; }
        #endregion

        #region ShowWizard
        [PrincipalPermission(SecurityAction.Demand, Role = "Administrator")]
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

                    if (App.Current.Windows.OfType<MainWindow>().FirstOrDefault() is MainWindow)
                    {


                        var progress = new Progress<int>(value => (App.Current.Windows.OfType<MainWindow>().FirstOrDefault()).progressBar.Value = value);
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

        #endregion

        #region CreateDestination
        private int CreateDestination()
        {
            var cnf = new ConfigurationData();
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

        #endregion

        //*******************************************************************
        private int CreateDestinationX()
        {
            var cnf = new ConfigurationData();
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
                var destMngr = new DestinationManager();
                var result = destMngr.CreateDestinationList(DriveLetter);
                dm.BulkCopy(cnf.GetConnectionString(), result.Result, driveId);
                //TODO: Создание Списка Сущностей в расположении
                var dest = new Destination(driveId, result.Result);
                //TODO: Добавление Медиа информации, если выбрана
                //TODO: Добавление картинок, если выбрана


                /*  
                   dm.FillDirectoriesInfo(driveId, DriveLetter);
                   dm.FillFilesInfo(driveId, DriveLetter);
                   dm.ClearCash(); 
                */
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(dm.logger.GetLog());
            }
            return driveId;
        }


      

    }



}
