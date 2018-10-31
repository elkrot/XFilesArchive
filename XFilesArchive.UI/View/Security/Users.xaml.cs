using Autofac;
using System.Threading.Tasks;
using System.Windows.Controls;
using XFilesArchive.UI.Startup;
using XFilesArchive.UI.ViewModel.Security;

namespace XFilesArchive.UI.View.Security
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : UserControl
    {
        public Users()
        {
            InitializeComponent();
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var usersViewModel = container.Resolve<UsersViewModel>();
            Task newTask = new Task(async delegate () {
                await usersViewModel.LoadAsync(0);
            });

            newTask.RunSynchronously();
            DataContext = usersViewModel;

        }
    }
}
