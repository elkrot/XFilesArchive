using MahApps.Metro.Controls;
using XFilesArchive.UI.ViewModel.Security;

namespace XFilesArchive.UI.View.Security
{
    public interface IView
        {
            IViewModel ViewModel
            {
                get;
                set;
            }

            void Show();
        }



        /// <summary>
        /// Interaction logic for LoginWindow.xaml
        /// </summary>
        public partial class LoginWindow : MetroWindow, IView
        {

 public LoginWindow()
        {
            InitializeComponent();
        }
            public LoginWindow(AuthenticationViewModel viewModel)
            {
                ViewModel = viewModel;
                // DataContext = viewModel;
                InitializeComponent();
            }

            #region IView Members
            public IViewModel ViewModel
            {
                get { return base.DataContext as IViewModel; }
                set { base.DataContext = value; }
            }
            #endregion
        }
}
