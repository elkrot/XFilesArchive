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
using System.Windows.Shapes;
using XFilesArchive.Security;
using XFilesArchive.Security.Services;

namespace XFilesArchive.UI.View.Security
{
    /// <summary>
    /// Логика взаимодействия для FacebookLoginWindow.xaml
    /// </summary>
    /// 

   
    public partial class FacebookLoginWindow : Window
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
