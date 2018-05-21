using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;
using XFilesArchive.UI.View;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow :  MetroWindow
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
         /*   _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;*/

            InitializeComponent();
            Main.Content = new DrivePage(viewModel);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           await _viewModel.LoadAsync();
        }


        void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }
    }
}
