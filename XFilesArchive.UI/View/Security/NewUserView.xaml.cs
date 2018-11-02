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
