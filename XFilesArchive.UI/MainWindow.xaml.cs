using MahApps.Metro.Controls;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Input;
using XFilesArchive.UI.View;
using XFilesArchive.UI.ViewModel;

namespace XFilesArchive.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
     [PrincipalPermission(SecurityAction.Demand)]
    ///[PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    public partial class MainWindow :  MetroWindow
    {    
        //private MetroWindow _window;

        public MainWindow(MainViewModel viewModel, MainNavigationViewModel mainNav)
        {
            InitializeComponent();
            Navigator.Content = new NavigationPage(mainNav);
            Main.Content = new DrivePage(viewModel);
        }

        void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // ShowWizard();
            Help.ShowHelp(null, "XFilesArchive.chm", HelpNavigator.TopicId, "1234");
            //System.Diagnostics.Process.Start("notepad.exe");
        }


    }
}
