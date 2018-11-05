using MahApps.Metro.Controls;
using System.Windows;
using XFilesArchive.Security;
using XFilesArchive.Security.Services;

namespace XFilesArchive.UI.View.Security
{
    /// <summary>
    /// Логика взаимодействия для FacebookLoginWindow.xaml
    /// </summary>
    /// 


    public partial class FacebookLoginWindow : MetroWindow
    {

        public UserDto User;
        FacebookAuthenticationService _service;
        public FacebookLoginWindow()
        {
            User = new UserDto("","",null);
            InitializeComponent();
            _service = new FacebookAuthenticationService(fbBrowser);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            _service.AuthenticateUser();
            fbBrowser.Navigated += FbBrowser_Navigated;
        }

        private void FbBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            User.Username = _service.UserName;
            User.Email = _service.Email;
            this.Close();
        }
    }
}
