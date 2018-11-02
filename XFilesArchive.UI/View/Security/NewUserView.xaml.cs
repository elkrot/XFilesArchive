using System.Windows;
using XFilesArchive.UI.Wrapper;

namespace XFilesArchive.UI.View.Security
{
    /// <summary>
    /// Логика взаимодействия для NewUserView.xaml
    /// </summary>
    public partial class NewUserView : Window
    {
        public UserDtoWrapper UserDto { get; set; }
        public NewUserView(UserDtoWrapper user)
        {
            InitializeComponent();
            UserDto = user;
            DataContext = user;
        }
    }
}
