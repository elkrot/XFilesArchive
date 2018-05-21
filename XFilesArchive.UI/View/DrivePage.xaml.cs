using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI.View
{
    /// <summary>
    /// Логика взаимодействия для DrivePage.xaml
    /// </summary>
    public partial class DrivePage : Page
    {
        private MainViewModel _viewModel;

        public DrivePage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            _viewModel = mainViewModel;
            DataContext = mainViewModel;
            Loaded += DrivePage_Loaded;
        }

        private async void DrivePage_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
