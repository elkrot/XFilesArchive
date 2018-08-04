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
    /// Логика взаимодействия для NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {

        private MainNavigationViewModel _viewModel;


        public NavigationPage(MainNavigationViewModel mainViewModel)
        {
            InitializeComponent();

            InputBindings.Add(new KeyBinding(mainViewModel.NewDestinationCommand, 
                new KeyGesture(Key.Z, ModifierKeys.Control)));
            _viewModel = mainViewModel;
            DataContext = mainViewModel;
            Loaded += NavigationPage_Loaded;
        }

        private void NavigationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            foreach (InputBinding ib in this.InputBindings)
            {
                window.InputBindings.Add(ib);
            }
        }
    }
}
