using MahApps.Metro.Controls;
using System.Windows;
using XFilesArchive.UI.Wrapper;

namespace XFilesArchive.UI.View.Security
{
    /// <summary>
    /// Логика взаимодействия для NewUserView.xaml
    /// </summary>
    public partial class NewUserView : MetroWindow
    {
        public UserDtoWrapper UserDto { get; set; }
        public NewUserView(UserDtoWrapper user)
        {
            InitializeComponent();
            UserDto = user;
            DataContext = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
